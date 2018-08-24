using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Skills
{
    [CreateAssetMenu(fileName = "Stone", menuName = "Skills/Stone")]
    public class skill_Stone : Skill
    {


    }

    [CustomEditor(typeof(skill_Stone))]
    [CanEditMultipleObjects]
    public partial class skill_Stone_window : SkillBrowser
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}