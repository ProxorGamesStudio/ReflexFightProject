using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Skills
{
    [CreateAssetMenu(fileName = "Dagger", menuName = "Skills/Dagger")]
    public class skill_Dagger : Skill
    {


    }

    [CustomEditor(typeof(skill_Dagger))]
    [CanEditMultipleObjects]
    public partial class skill_Dagger_window : SkillBrowser
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}