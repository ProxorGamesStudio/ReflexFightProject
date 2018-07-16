using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Profiles
{
    [CreateAssetMenu(fileName = "Some Profile", menuName = "Profiles/Controller", order = 1)]

    [System.Serializable]
    public class ControllerProfile : ScriptableObject
    {
        public float MoveSpeed, AngularSpeed, acceleration, stoppingDistance;
        public float stopAnimDistance;
        public GameObject pointerEffect;
        public List<Skill> Skills;
        public int gridNum;
        public Transform Canvas;
        public GameObject skillExamplePrefab;
        public Color skillsColor, skillsCooldownColor, skillsCooldownTextColor;
        public Font skillsFont;
        public int fontSize, skilltype;
        public Texture2D textIco, paramIco, cooldownIco;

        public ControllerProfile()
        {

        }
    }
}
