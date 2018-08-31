using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Skills
{
    [CreateAssetMenu(fileName = "Shock Wave", menuName = "Skills/Shock Wave")]
    public class skill_ShockWave : Skill
    {

    }

    [CustomEditor(typeof(skill_ShockWave))]
    [CanEditMultipleObjects]
    public partial class skill_ShockWave_window : SkillBrowser
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}