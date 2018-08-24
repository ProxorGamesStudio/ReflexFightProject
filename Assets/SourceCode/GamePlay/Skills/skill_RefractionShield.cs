using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Skills
{
    [CreateAssetMenu(fileName = "Refraction Shiled", menuName = "Skills/Refraction Shield")]
    public class skill_RefractionShield : Skill
    {

    }
    [CustomEditor(typeof(skill_RefractionShield))]
    [CanEditMultipleObjects]
    public partial class skill_RefractionShield_window : SkillBrowser
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}