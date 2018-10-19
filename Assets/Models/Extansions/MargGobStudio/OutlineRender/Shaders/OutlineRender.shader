Shader "Hidden/MargGob/OutlineRender"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{ Name "Outline Render"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragAO
			#pragma target 2.0
			#define UNITY_GBUFFER_INCLUDED
			#include "UnityCG.cginc"
			#include "UnityGBuffer.cginc"

			// System built-in variables
			sampler2D _MainTex;
			sampler2D_float _CameraDepthTexture;
			sampler2D _CameraDepthNormalsTexture;
			float4 _CameraDepthTexture_ST;
			// Shader variables
			int _lineThickness;

			struct appdata	
			{
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
			};

			struct v2f	
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)	
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				return o;
			}

			float SampleDepthNormal(float2 uv,  out float3 normal){
			#if defined(SOURCE_GBUFFER) || defined(SOURCE_DEPTH)
			    normal = SampleNormal(uv);
			    return SampleDepth(uv);
			#else
			    float4 cdn = tex2D(_CameraDepthNormalsTexture, uv);
			    normal = DecodeViewNormalStereo(cdn) * float3(1.0, 1.0, -1.0);
			    float4 d = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, uv));
			    return LinearEyeDepth(d.a);
			#endif
			}

			float3 ReconstructViewPos(float2 uv, float depth, float2 p11_22, float2 p13_31){
			    return float3((uv * 2.0 - 1.0 - p13_31) / p11_22 * lerp(depth, 1.0, unity_OrthoParams.w), depth);
			}

			fixed4 fragAO (v2f i) : SV_Target{ 

				//ScreenUV
				float2 uv = i.uv;

				float4 c = tex2D(_MainTex, uv);

				//Normal and Depth
				float4 nrmDepth = 0; //XYZ - normal, A - depth
			    nrmDepth.a = SampleDepthNormal(UnityStereoScreenSpaceUVAdjust(uv, _CameraDepthTexture_ST), nrmDepth.xyz);
			    float depthLerp = step(_ProjectionParams.z, nrmDepth.a);

				//Position
				const float2 p11_22 = (unity_CameraProjection._11, unity_CameraProjection._22);
				const float2 p13_31 = (unity_CameraProjection._13, unity_CameraProjection._23);
				float3 position = ReconstructViewPos(uv, nrmDepth.a, p11_22, p13_31);
				float3 viewDir = normalize(position);

				//Outline
				float3 ao = 0; 
				float samples = 8;

				const float2 sampleSize[8] = {
				1,1,	-1,1,					
				-1,-1,	1,-1,
				0,1,	1,0,
				-1,0,	0,-1,
				};

				for(int i = 0; i < samples; i++){

					//New UV
	                float2 newUV = uv + (sampleSize[i]) / _ScreenParams.xy * _lineThickness;
	                //Normal and Depth
	                float4 newNrmDepth = 0; //XYZ - normal, A - depth
	                newNrmDepth.a = SampleDepthNormal(UnityStereoScreenSpaceUVAdjust(newUV, _CameraDepthTexture_ST), newNrmDepth.xyz);
	                float3 sampledPosition = ReconstructViewPos (newUV, newNrmDepth.a, p11_22, p13_31);	                		

	                //Calculate Curvature map and outline
	                float3 sampledDir = normalize(position - sampledPosition);
	                float d0 = dot(nrmDepth.xyz, sampledDir);
	                float d1 = dot(viewDir, sampledDir);
	                float d2 = dot((nrmDepth.xyz - newNrmDepth.xyz), sampledDir);

	                ao.x += d0;
	                ao.y += d2;
	                ao.z += d1;
				}

				ao /= samples;
				ao.x = (ao.x + ao.y) * 0.5 + 0.5;
				ao.y = pow(smoothstep(0, 0.46, ao.x), 8);
				ao.x = lerp(pow(smoothstep(0.60, 1, ao.x), 0.0675), 0,  depthLerp);
				ao.z = 1 - smoothstep(0,1, ao.z);

				//Adding light edges
				c.rgb += c.rgb * ao.x * 0.8;
				//Add darkening in the grooves and around the edges
				c.rgb *= (ao.y * ao.z + 0.2) / 1.2;

				return c;
			}

		ENDCG
	}
}
}
