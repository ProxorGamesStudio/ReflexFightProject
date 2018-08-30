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
[CreateAssetMenu(fileName = "New Hit Effect", menuName = "Surface/Surface Hit Effect")]
public class SurfaceHitEffect : ScriptableObject
{
#if UNITY_EDITOR
    public ReorderableList goList = null;
#endif
    public List<GameObject> HitEffects = null;
    public Vector3 LocalEffectRotation;
    public bool RandomRotation;
    public float DestroyTimeout = 0f;
}

#if UNITY_EDITOR
[CustomEditor(typeof(SurfaceHitEffect))]
public class SurfaceHitEffectInspector : Editor
{
    SurfaceHitEffect window;

    void OnEnable()
    {
        window = target as SurfaceHitEffect;

        if (window.HitEffects == null)
            window.HitEffects = new List<GameObject>();


        window.goList = new ReorderableList(window.HitEffects, typeof(AudioClip), true, true, true, true);
        window.goList.elementHeight = 16;

        window.goList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            window.HitEffects[index] = (GameObject)EditorGUI.ObjectField(rect, "Hit effect " + index, window.HitEffects[index], typeof(GameObject), false);
        };

        window.goList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Hit Effects:");
        };
        window.goList.onAddCallback = (list) =>
        {
            window.HitEffects.Add(null);
        };

        window.goList.onRemoveCallback = (list) =>
        {
            window.HitEffects.RemoveAt(list.index);
        };
    }

    public override void OnInspectorGUI()
    {
        GUIStyle Header = new GUIStyle(EditorStyles.boldLabel);
        Header.alignment = TextAnchor.LowerCenter;
        Header.fontSize = 15;

        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField(window.name, Header, GUILayout.Height(20));

            GUILayout.Space(10);

            window.LocalEffectRotation = EditorGUILayout.Vector3Field("Local effect rotation", window.LocalEffectRotation);
            window.RandomRotation = EditorGUILayout.Toggle("Random rotation", window.RandomRotation);
            window.DestroyTimeout = EditorGUILayout.FloatField("Destroy timeout" ,window.DestroyTimeout);

            GUILayout.Space(10);

            window.goList.DoLayoutList();

            if (GUILayout.Button("Save", "prebutton", GUILayout.Height(20)))
            {
                EditorUtility.SetDirty(window);
                AssetDatabase.SaveAssets();
            }
        }
        EditorGUILayout.EndVertical();
    }
}
#endif
