using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(Terrain))]
public class SurfaceTerrainHitInfo : MonoBehaviour, ISurfaceHitInfo
{
    internal TerrainData terrainData;
    internal Terrain terrain;

    private int alphamapWidth;
    private int alphamapHeight;
    private int numTextures;

    private float[,,] splatmapData;

    [SerializeField]
    public List<SurfaceChoiceInfo> selectChoicesList = new List<SurfaceChoiceInfo>();
    [SerializeField]
    public SurfaceProperties[] surfaceProperties;


    private void Awake()
    {
        terrain = GetComponent<Terrain>();

        terrainData = terrain.terrainData;
        alphamapWidth = terrainData.alphamapWidth;
        alphamapHeight = terrainData.alphamapHeight;

        splatmapData = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight);
    }

    public SurfaceProperties GetSurfaceProperties(Vector3 castPos)
    {
        return surfaceProperties[GetActiveTerrainTextureIdx(castPos)];
    }

    private Vector3 ConvertToSplatMapCoordinate(Vector3 playerPos)
    {
        Vector3 vecRet = new Vector3();
        vecRet.x = ((playerPos.x - terrain.transform.position.x) / terrainData.size.x) * terrainData.alphamapWidth;
        vecRet.z = ((playerPos.z - terrain.transform.position.z) / terrainData.size.z) * terrainData.alphamapHeight;
        return vecRet;
    }

    private int GetActiveTerrainTextureIdx(Vector3 castPos)
    {
        Vector3 playerPos = castPos;
        Vector3 TerrainCord = ConvertToSplatMapCoordinate(playerPos);
        int ret = 0;
        float comp = 0f;
        for (int i = 0; i < numTextures; i++)
        {
            if (comp < splatmapData[(int)TerrainCord.z, (int)TerrainCord.x, i])
                ret = i;
        }
        return ret;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SurfaceTerrainHitInfo))]
[CanEditMultipleObjects]
public class SurfaceTerrainInspector : Editor
{
    private SurfaceTerrainHitInfo window;

    private string[] listSurfaces;
    private SurfaceProperties[] surfacePropertiesFiles;


    private void OnEnable()
    {
        window = target as SurfaceTerrainHitInfo;

        listSurfaces = AssetDatabase.FindAssets("t:SurfaceProperties");
        surfacePropertiesFiles = new SurfaceProperties[listSurfaces.Length];

        for (int i = 0; i < listSurfaces.Length; i++)
        {
            surfacePropertiesFiles[i] = AssetDatabase.LoadAssetAtPath<SurfaceProperties>(AssetDatabase.GUIDToAssetPath(listSurfaces[i]));
            listSurfaces[i] = surfacePropertiesFiles[i].surfaceName;
        }
    }



    public override void OnInspectorGUI()
    {
        if (listSurfaces.Length != 0)
        {
            window.surfaceProperties = new SurfaceProperties[window.terrainData.splatPrototypes.Length];

            List<SurfaceChoiceInfo> new_surfaceChoiceInfos = new List<SurfaceChoiceInfo>();
            for (int i = 0; i < window.terrainData.splatPrototypes.Length; i++)
            {
                SurfaceChoiceInfo s = window.selectChoicesList.Find(x => x.texture == window.terrainData.splatPrototypes[i].texture);

                if (s != null)
                {
                    if (s.selected >= listSurfaces.Length)
                        s.selected = 0;
                    new_surfaceChoiceInfos.Add(s);
                }
                else
                    new_surfaceChoiceInfos.Add(new SurfaceChoiceInfo(window.terrainData.splatPrototypes[i].texture, 0));
            }
            window.selectChoicesList = new_surfaceChoiceInfos;

            if (window.selectChoicesList.Count > 0)
            {
                EditorGUILayout.BeginVertical();
                {
                    GUILayout.Space(20);
                    for (int i = 0; i < window.selectChoicesList.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.Box(window.selectChoicesList[i].texture, GUILayout.Width(32), GUILayout.Height(32));

                            EditorGUILayout.BeginVertical();
                            EditorGUILayout.LabelField("Surface properties for: " + window.selectChoicesList[i].texture.name);
                            window.selectChoicesList[i].selected = EditorGUILayout.Popup(window.selectChoicesList[i].selected, listSurfaces);
                            EditorGUILayout.EndVertical();
                            window.surfaceProperties[i] = surfacePropertiesFiles[window.selectChoicesList[i].selected];
                        }
                        EditorGUILayout.EndHorizontal();

                        GUILayout.Space(20);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            else
                EditorGUILayout.HelpBox("Warning! Terrain has no textures!", MessageType.Warning);
        }
        else
            EditorGUILayout.HelpBox("Warning! Could not find any surface properties file!", MessageType.Warning);

    }
}
#endif

[Serializable]
public class SurfaceChoiceInfo
{
    public Texture2D texture;
    public int selected;

    public SurfaceChoiceInfo(Texture2D texture, int selected)
    {
        this.texture = texture;
        this.selected = selected;
    }
}