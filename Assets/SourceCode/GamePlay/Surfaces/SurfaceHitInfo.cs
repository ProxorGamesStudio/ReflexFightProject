using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Collider))]
public class SurfaceHitInfo: MonoBehaviour, ISurfaceHitInfo
{
    [SerializeField]
    public SurfaceProperties surfaceProperties;
    [SerializeField]
    public int selected;

    public SurfaceProperties GetSurfaceProperties(Vector3 castPos)
    {
        return surfaceProperties;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SurfaceHitInfo))]
[CanEditMultipleObjects]
public class SurfaceInspector : Editor
{
    private SurfaceHitInfo window;

    private string[] listSurfaces;
    private SurfaceProperties[] surfacePropertiesFiles;

    

    private void OnEnable()
    {
        window = target as SurfaceHitInfo;

        listSurfaces = AssetDatabase.FindAssets("t:SurfaceProperties");
        surfacePropertiesFiles = new SurfaceProperties[listSurfaces.Length];

        for (int i = 0; i < listSurfaces.Length; i++)
        {
            surfacePropertiesFiles[i] = AssetDatabase.LoadAssetAtPath<SurfaceProperties>(AssetDatabase.GUIDToAssetPath(listSurfaces[i]));
            listSurfaces[i] = surfacePropertiesFiles[i].surfaceName;
        }

        if (window.selected >= listSurfaces.Length)
            window.selected = 0;
    }

    public override void OnInspectorGUI()
    {

        if (listSurfaces.Length != 0)
        {
            window.selected = EditorGUILayout.Popup("Surface properties: ", window.selected, listSurfaces);
            window.surfaceProperties = surfacePropertiesFiles[window.selected];
        }
        else
            EditorGUILayout.HelpBox("Warning! Could not find any surface properties file!", MessageType.Warning);
    }
}
#endif