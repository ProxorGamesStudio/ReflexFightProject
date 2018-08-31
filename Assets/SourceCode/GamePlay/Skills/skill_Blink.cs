using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Skills
{
    [CreateAssetMenu(fileName = "Blink", menuName = "Skills/Blink")]
    public class skill_Blink : Skill
    {


    }

    [CustomEditor(typeof(skill_Blink))]
    [CanEditMultipleObjects]
    public partial class skill_Blink_window : SkillBrowser
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}