using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

[System.Serializable]
#if UNITY_EDITOR
[CanEditMultipleObjects]
#endif
[CreateAssetMenu(fileName = "New Surface", menuName = "Surface/Surface Properties File")]
public class SurfaceProperties : ScriptableObject
{

    public enum DirectionType { ImpactSourse, SurfaceNormal }

    public string surfaceName = string.Empty;

#if UNITY_EDITOR
    public ReorderableList stepList = null;
    public ReorderableList bulletList = null;

    public ReorderableList effectStepList = null;
    public ReorderableList effectBulletList = null;
#endif


    #region Steps
    [SerializeField]
    public List<AudioClip> StepSounds = null;

    [SerializeField]
    public List<SurfaceHitEffect> StepEffects = null;

    public void Step_PlayHitEffect(AudioSource source)
    {
        try
        {
            source.PlayOneShot(StepSounds[UnityEngine.Random.Range(0, StepSounds.Count)]);
        }
        catch
        {
            Debug.LogWarning("You don't assign foot steps sounds!");
        }
    }
    #endregion

    #region Bullets
    [SerializeField]
    public List<AudioClip> BulletSounds;

    [SerializeField]
    public List<SurfaceHitEffect> BulletEffects;

    [SerializeField]
    public float skin = 0.5f;

    [SerializeField]
    public DirectionType directionType;

    public void Bullet_PlayHitEffect(Vector3 hitPoint, Vector3 fromPoint, Vector3 normal)
    {
        
    }

    #endregion


}

#if UNITY_EDITOR
[CustomEditor(typeof(SurfaceProperties))]
public class SurfacePropertiesInspector : Editor
{
    SurfaceProperties window;

    private bool fadeSteps, fadeBullets;

    void OnEnable()
    {
        window = target as SurfaceProperties;
        if (window.surfaceName == string.Empty)
        {
            window.surfaceName = window.name;
            EditorUtility.SetDirty(window);
            AssetDatabase.SaveAssets();
        }

        if (window.StepSounds == null)
            window.StepSounds = new List<AudioClip>();


        window.stepList = new ReorderableList(window.StepSounds, typeof(AudioClip), true, true, true, true);
        window.stepList.elementHeight = 16;

        window.stepList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            window.StepSounds[index] = (AudioClip)EditorGUI.ObjectField(rect, "Sound " + index, window.StepSounds[index], typeof(AudioClip), false);
        };

        window.stepList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Foot steps hit sounds:");
        };
        window.stepList.onAddCallback = (list) =>
        {
            window.StepSounds.Add(null);
        };

        window.stepList.onRemoveCallback = (list) =>
        {
            window.StepSounds.RemoveAt(list.index);
        };


        if (window.BulletSounds == null)
            window.BulletSounds = new List<AudioClip>();


        window.bulletList = new ReorderableList(window.BulletSounds, typeof(AudioClip), true, true, true, true);
        window.bulletList.elementHeight = 16;

        window.bulletList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            window.BulletSounds[index] = (AudioClip)EditorGUI.ObjectField(rect, "Sound " + index, window.BulletSounds[index], typeof(AudioClip), false);
        };

        window.bulletList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Bullet hit sounds:");
        };
        window.bulletList.onAddCallback = (list) =>
        {
            window.BulletSounds.Add(null);
        };

        window.bulletList.onRemoveCallback = (list) =>
        {
            window.BulletSounds.RemoveAt(list.index);
        };



        if (window.StepEffects == null)
            window.StepEffects = new List<SurfaceHitEffect>();


        window.effectStepList = new ReorderableList(window.StepEffects, typeof(SurfaceHitEffect), true, true, true, true);
        window.effectStepList.elementHeight = 16;

        window.effectStepList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            window.StepEffects[index] = (SurfaceHitEffect)EditorGUI.ObjectField(rect, "Effect " + index, window.StepEffects[index], typeof(SurfaceHitEffect), false);
        };

        window.effectStepList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Foot steps hit effects:");
        };
        window.effectStepList.onAddCallback = (list) =>
        {
            window.StepEffects.Add(null);
        };

        window.effectStepList.onRemoveCallback = (list) =>
        {
            window.StepEffects.RemoveAt(list.index);
        };


        if (window.BulletEffects == null)
            window.BulletEffects = new List<SurfaceHitEffect>();


        window.effectBulletList = new ReorderableList(window.BulletEffects, typeof(SurfaceHitEffect), true, true, true, true);
        window.effectBulletList.elementHeight = 16;

        window.effectBulletList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            window.BulletEffects[index] = (SurfaceHitEffect)EditorGUI.ObjectField(rect, "Effect " + index, window.BulletEffects[index], typeof(SurfaceHitEffect), false);
        };

        window.effectBulletList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Bullet hit effetcs:");
        };
        window.effectBulletList.onAddCallback = (list) =>
        {
            window.BulletEffects.Add(null);
        };

        window.effectBulletList.onRemoveCallback = (list) =>
        {
            window.BulletEffects.RemoveAt(list.index);
        };
    }

    public override void OnInspectorGUI()
    {
        GUIStyle Header = new GUIStyle(EditorStyles.boldLabel);
        Header.alignment = TextAnchor.LowerCenter;
        Header.fontSize = 16;

        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField("General:", Header, GUILayout.Height(20));
            window.surfaceName = EditorGUILayout.TextField("Surface name", window.surfaceName);

            GUILayout.Space(13);

            EditorGUILayout.LabelField("Foot steps Surface properties:", Header, GUILayout.Height(20));
            EditorGUILayout.LabelField("Sounds", EditorStyles.centeredGreyMiniLabel);
            if (window.stepList != null)
                window.stepList.DoLayoutList();

            GUILayout.Space(5);

            EditorGUILayout.LabelField("Effects", EditorStyles.centeredGreyMiniLabel);
            if (window.effectStepList != null)
                window.effectStepList.DoLayoutList();

            GUILayout.Space(10);

            EditorGUILayout.LabelField("Bullets Surface properties:", Header, GUILayout.Height(20));
            EditorGUILayout.LabelField("General", EditorStyles.centeredGreyMiniLabel);
            window.directionType = (SurfaceProperties.DirectionType)EditorGUILayout.EnumPopup("Direction Type", window.directionType);
            window.skin = EditorGUILayout.FloatField("Skin", window.skin);
            EditorGUILayout.LabelField("Sounds", EditorStyles.centeredGreyMiniLabel);
            if (window.bulletList != null)
                window.bulletList.DoLayoutList();

            GUILayout.Space(5);

            EditorGUILayout.LabelField("Effects", EditorStyles.centeredGreyMiniLabel);
            if (window.effectBulletList != null)
                window.effectBulletList.DoLayoutList();

            GUILayout.Space(10);

            if (GUILayout.Button("Save", "prebutton", GUILayout.Height(20)))
            {
                EditorUtility.SetDirty(window);
                AssetDatabase.SaveAssets();
            }
        }
        EditorGUILayout.EndVertical();
    }
}

[CustomEditor(typeof(Material))]
public class SurfacePropertiesMaterialInspector : MaterialEditor
{
    private Material window;
    private int selected = 0;
    private SurfaceProperties surfaceProperties = null;

    private string[] listSurfaces = new string[0];
    private SurfaceProperties[] surfacePropertiesFiles = new SurfaceProperties[0];

    public override void OnEnable()
    {
        base.OnEnable();

        window = target as Material;

        

        listSurfaces = AssetDatabase.FindAssets("t:SurfaceProperties");
        surfacePropertiesFiles = new SurfaceProperties[listSurfaces.Length];

        for (int i = 0; i < listSurfaces.Length; i++)
        {
            surfacePropertiesFiles[i] = AssetDatabase.LoadAssetAtPath<SurfaceProperties>(AssetDatabase.GUIDToAssetPath(listSurfaces[i]));
            listSurfaces[i] = surfacePropertiesFiles[i].surfaceName;
        }

        if (listSurfaces.Length != 0)
        {
            selected = 0;
            surfaceProperties = surfacePropertiesFiles[selected];
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUIStyle Header = new GUIStyle(EditorStyles.boldLabel);
        Header.alignment = TextAnchor.LowerCenter;
        Header.fontSize = 13;

        if (listSurfaces.Length != 0)
        {

            EditorGUILayout.BeginVertical();
            {
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Surface Tool", Header, GUILayout.Height(20));
                EditorGUILayout.HelpBox("Принцип работы данной утилиты: при нажатии кнопки \"Найти и приминить\" - будут найдены все объекты SurfaceHitInfo, и если на объекте с ними будет данный материал, " +
                    "то настройки с данного материла будут перенесены в SurfaceHitInfo", MessageType.Info);

                selected = EditorGUILayout.Popup("Surface properties: ", selected, listSurfaces);
                surfaceProperties = surfacePropertiesFiles[selected];

                GUILayout.Space(10);

                if (GUILayout.Button("Найти и приминить", "prebutton", GUILayout.Height(20)))
                {
                    MeshRenderer[] meshRenderers = FindObjectsOfType<MeshRenderer>();
                    foreach (MeshRenderer mr in meshRenderers)
                    {
                        if (mr.GetComponent<SurfaceHitInfo>() != null && mr.sharedMaterials.Length == 1)
                        {
                            mr.GetComponent<SurfaceHitInfo>().selected = selected;
                            mr.GetComponent<SurfaceHitInfo>().surfaceProperties = surfaceProperties;
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }
        else
            EditorGUILayout.HelpBox("Warning! Could not find any surface properties file!", MessageType.Warning);
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

}
#endif