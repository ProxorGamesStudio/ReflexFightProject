using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Skills
{

    [CreateAssetMenu(fileName = "Sword Attack", menuName = "Skills/Sword Attack")]
    public class skill_SwordAttack : Skill
    {
        public float AttackSpeed;
    }

    [CustomEditor(typeof(skill_SwordAttack))]
    [CanEditMultipleObjects]
    public partial class skill_SwordAttack_window : SkillBrowser
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var window = target as skill_SwordAttack;

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
                window.AttackSpeed = EditorGUILayout.FloatField(new GUIContent("Attack Speed", "Attack Speed"), window.AttackSpeed);
            }
            GUILayout.EndVertical();

        }
    }
}