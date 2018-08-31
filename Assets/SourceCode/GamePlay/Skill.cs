using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Skills
{

    public class Skill : ScriptableObject
    {
        public string name;
        public string discription;

        public float energyCost;

        public Sprite icon;

        public delegate Action skillAction<T>(params T[] info);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Skill))]
    [CanEditMultipleObjects]
    public partial class SkillBrowser : Editor
    {
        public override void OnInspectorGUI()
        {
            var window = target as Skill;

            GUIStyle Header = new GUIStyle(EditorStyles.boldLabel);
            Header.alignment = TextAnchor.LowerCenter;
            Header.fontSize = 16;

            GUIStyle Content = new GUIStyle();
            Content.fontSize = 13;

            GUIStyle skillHeader = new GUIStyle(EditorStyles.boldLabel);
            skillHeader.alignment = TextAnchor.LowerCenter;
            skillHeader.fontSize = 13;

            GUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("General Properties:", EditorStyles.centeredGreyMiniLabel);

                window.icon = (Sprite)EditorGUILayout.ObjectField("Skill Icon", window.icon, typeof(Sprite));
                window.name = EditorGUILayout.TextField(new GUIContent("Name", "Name of skill"), window.name);

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(new GUIContent("Description", "Description of skill"));
                    GUILayout.Space(-Screen.width/3);
                    window.discription = EditorGUILayout.TextArea(window.discription, GUILayout.Width(Screen.width/1.5f), GUILayout.Height(200));
                }
                GUILayout.EndHorizontal();
                window.energyCost = EditorGUILayout.FloatField(new GUIContent("Energy cost", "Energy cost by skill cast"), window.energyCost);
               
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Individual skill properies:", EditorStyles.centeredGreyMiniLabel);
            }
            GUILayout.EndVertical();

        }
    }
#endif
}