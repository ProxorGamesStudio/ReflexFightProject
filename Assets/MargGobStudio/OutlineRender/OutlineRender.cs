using System;
using UnityEngine;


namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
	[ImageEffectAllowedInSceneView]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu ("Image Effects/Marggob/OutlineRender")]
	public class OutlineRender : MonoBehaviour {

		private enum Mode {OnePixel, TwoPixels, FourPixels};
		#region Variables
		[SerializeField]
		private Mode lineThickness;
		private Material curMaterial;
		#endregion

		#region Properties
		Material material{
			get{ 
				if(curMaterial == null) 
				{
					curMaterial = new Material(Shader.Find("Hidden/MargGob/OutlineRender"));
					curMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return curMaterial; 
			}
		}
		#endregion

		void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture){
			if (lineThickness == Mode.OnePixel) 
				material.SetInt ("_lineThickness", 1);
			else if (lineThickness == Mode.TwoPixels) 
				material.SetInt ("_lineThickness", 2);
			else if (lineThickness == Mode.FourPixels) 
				material.SetInt ("_lineThickness", 4);
			Graphics.Blit (sourceTexture, destTexture, material);
		}
		
		// Update is called once per frame
		void Update () {
			Camera.main.depthTextureMode = DepthTextureMode.DepthNormals;


		}

		void OnDisable(){
			if(curMaterial){
					DestroyImmediate(curMaterial);
				}
		}
	}
}