using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [ImageEffectAllowedInSceneView]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Marggob/OutlineRender")]
    public class OutlineRender : MonoBehaviour
    {

        #region Variables

        private Material curMaterial;

        [SerializeField]
        [Range(0, 1)]
        private float blackLinePower = 0.6f;
        [SerializeField]
        [Range(0, 1)]
        private float whiteLinePower = 0.6f;
        [SerializeField]
        private bool debugMode = false;

        #endregion

        #region Properties

        Material material
        {
            get
            { 
                if (curMaterial == null)
                {
                    curMaterial = new Material(Shader.Find("Hidden/MargGob/OutlineRender"));
                    curMaterial.hideFlags = HideFlags.HideAndDontSave;
                }
                return curMaterial; 
            }
        }

        #endregion

        void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
        {
            Graphics.Blit(sourceTexture, destTexture, material);
        }

        void Update()
        {
            
            Camera.main.depthTextureMode = DepthTextureMode.DepthNormals;

            int lineThickness = Mathf.RoundToInt(Screen.height / 1080);
            if (lineThickness < 1)
            {
                lineThickness = 1;
            }
            material.SetInt("_lineThickness", lineThickness);

            if (debugMode == true)
            {
                material.SetFloat("_DebugMode", 1);
            }
            else
            {
                material.SetFloat("_DebugMode", 0);
            }

            material.SetFloat("_blackLinePower", blackLinePower);
            material.SetFloat("_whiteLinePower", whiteLinePower);


        }

        void OnDisable()
        {
            if (curMaterial)
            {
                DestroyImmediate(curMaterial);
            }
        }
    }
}