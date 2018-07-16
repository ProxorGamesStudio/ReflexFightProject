using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Controller))]
[System.Serializable]
public class SkillsBrowser : Editor{

    public override void OnInspectorGUI()
    {
        Controller window = target as Controller;

        GUIStyle Header = new GUIStyle(EditorStyles.boldLabel);
        Header.alignment = TextAnchor.LowerCenter;
        Header.fontSize = 16;

        GUIStyle skillHeader = new GUIStyle(EditorStyles.boldLabel);
        skillHeader.alignment = TextAnchor.LowerCenter;
        skillHeader.fontSize = 13;

        GUIStyle Content = new GUIStyle();
        Content.fontSize = 13;

        GUILayout.BeginVertical();

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Character settings:", Header, GUILayout.Height(20));
        EditorGUILayout.LabelField("Attached components:", EditorStyles.centeredGreyMiniLabel);
        GUILayout.Space(3);
        window.animator = (Animator)EditorGUILayout.ObjectField("Animator", window.animator, typeof(Animator));
        window.setControllerHide= EditorGUILayout.Toggle(new GUIContent("Hide NavMesh Agent", "Скрывает компонент навмеш-агента из инспектора - для удобности и читаемости"), window.setControllerHide);
        window.Canvas = (Transform)EditorGUILayout.ObjectField(new GUIContent("Canvas rect", "Элемент канваса куда будут писать и откуда будут браться скиллы"), window.Canvas, typeof(Transform));
        if (!window.setControllerHide)
            window.controller.hideFlags = HideFlags.None;
        else
            window.controller.hideFlags = HideFlags.HideInInspector;

        EditorGUILayout.LabelField("Main params:", EditorStyles.centeredGreyMiniLabel);
        GUILayout.Box("", GUILayout.Height(2), GUILayout.Width(Screen.width / 1.1f));
        window.gridNum = GUILayout.SelectionGrid(window.gridNum, new GUIContent[] { new GUIContent("Use Profile", "Использовать созданый профиль игрока из ассетов"), new GUIContent("Use native settings", "Настроить уникальные параметры для конкретного случая") }, 2);
        GUILayout.Box("", GUILayout.Height(2), GUILayout.Width(Screen.width / 1.1f));
        GUILayout.Space(5);
        if (window.gridNum == 1)
        {
            window.profile = (Profiles.ControllerProfile)EditorGUILayout.ObjectField("Profile", window.profile, typeof(Profiles.ControllerProfile));
            if (GUILayout.Button(new GUIContent("Copy from Profile", "Скопировать настройки из профиля"), GUILayout.Width(Screen.width / 4)) && window.profile)
            {
                window.speed = window.profile.MoveSpeed;
                window.stopAnimDistance = window.profile.stopAnimDistance;
                window.skills = window.profile.Skills;
            }

            window.speed = EditorGUILayout.FloatField(new GUIContent("Move speed", "Скорость, с который будет двигаться персонаж"), window.speed);
            window.rotationSpeed = EditorGUILayout.FloatField(new GUIContent("Rotation Speed", "Скорость, с который будет поворачиваться персонаж"), window.rotationSpeed);
            window.gravity = EditorGUILayout.FloatField(new GUIContent("Gravitation", "Гравитация"), window.gravity);
            EditorGUILayout.LabelField("Animation settings", EditorStyles.centeredGreyMiniLabel);
            window.SmoothRotation = EditorGUILayout.FloatField(new GUIContent("Rotation smooth", "Сглаживание анимации поворта"), window.SmoothRotation);
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Skills:", Header, GUILayout.Height(20));
            GUILayout.Space(10);
            window.skillExamplePrefab = (GameObject)EditorGUILayout.ObjectField("Example skill prefab", window.skillExamplePrefab, typeof(GameObject));

            EditorGUILayout.LabelField("General params:", EditorStyles.centeredGreyMiniLabel);
            window.skillsColor = EditorGUILayout.ColorField("Skills main color", window.skillsColor);
            window.skillsCooldownColor = EditorGUILayout.ColorField("Skills cooldown color", window.skillsCooldownColor);
            window.skillsCooldownTextColor = EditorGUILayout.ColorField("Skills cooldown timer color", window.skillsCooldownTextColor);
            window.skillsFont = (Font)EditorGUILayout.ObjectField("Cooldown timer font", window.skillsFont, typeof(Font));
            window.fontSize = EditorGUILayout.IntField("Cooldown timer font size", window.fontSize);
            GUILayout.Space(5);
           
            if(!window.textIco)
                window.textIco = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/text.png", typeof(Texture2D));
            if(!window.paramIco)
                window.paramIco = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/Param.png", typeof(Texture2D));
            if (!window.cooldownIco)
                window.cooldownIco = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/Cooldown.png", typeof(Texture2D));
            EditorGUILayout.LabelField("Skills list:", EditorStyles.centeredGreyMiniLabel);

            for (int i = 0; i < window.skills.Count; i++)
            {
                GUILayout.Box("", GUILayout.Height(2), GUILayout.Width(Screen.width / 1.1f));
                EditorGUILayout.LabelField(window.skills[i].SkillName, skillHeader);
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                window.skills[i].icon = (Sprite)EditorGUILayout.ObjectField(window.skills[i].icon,  typeof(Sprite), false, GUILayout.Width(65f), GUILayout.Height(65f));
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Type of skill", GUILayout.Width(Screen.width / 3f));
                window.skilltype = EditorGUILayout.IntPopup(window.skilltype, new string[] { "Directed on target", "Directed at range", "Directed at heimself" }, new int[] { 0, 1, 2 }, GUILayout.Width(Screen.width / 2));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(window.textIco, GUILayout.Width(16), GUILayout.Height(16));
                window.skills[i].SkillName = EditorGUILayout.TextField("Skill name: ",window.skills[i].SkillName);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(window.paramIco, GUILayout.Width(16), GUILayout.Height(16));
                window.skills[i].Param = EditorGUILayout.FloatField("Factor: ", window.skills[i].Param);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(window.cooldownIco, GUILayout.Width(16), GUILayout.Height(16));
                window.skills[i].Cooldown = EditorGUILayout.FloatField("Cooldown: ", window.skills[i].Cooldown);
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("-", "Удалить скилл"), GUILayout.Width(Screen.width / 4)))
                {
                    window.skills.RemoveAt(i);
                    break;
                }
                window.skills[i].InvokedVoid = EditorGUILayout.TextField("Invoke call: ", window.skills[i].InvokedVoid);
                GUILayout.EndHorizontal();
                GUILayout.Box("", GUILayout.Height(2), GUILayout.Width(Screen.width / 1.1f));
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(Screen.width / 2 - Screen.width/6);
            if (GUILayout.Button(new GUIContent("+", "Создать новый скилл"), GUILayout.Width(Screen.width/3)))
                window.skills.Add(new Skill("Some skill"));

            GUILayout.Space(15);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

        }
        else
        {
            window.profile = (Profiles.ControllerProfile)EditorGUILayout.ObjectField("Profile", window.profile, typeof(Profiles.ControllerProfile));
            if (window.profile)
            {
                window.speed = window.profile.MoveSpeed;   
            }
        }
    }

    internal void Skills()
    {

    }
}

[CustomEditor(typeof(Profiles.ControllerProfile))]
public class ControllerBrowser : Editor
{
    public override void OnInspectorGUI()
    {
        Profiles.ControllerProfile window = target as Profiles.ControllerProfile;

        GUIStyle Header = new GUIStyle(EditorStyles.boldLabel);
        Header.alignment = TextAnchor.LowerCenter;
        Header.fontSize = 16;

        GUIStyle Content = new GUIStyle();
        Content.fontSize = 13;

        GUIStyle skillHeader = new GUIStyle(EditorStyles.boldLabel);
        skillHeader.alignment = TextAnchor.LowerCenter;
        skillHeader.fontSize = 13;

        GUILayout.BeginVertical();

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Character settings:", Header, GUILayout.Height(20));
        EditorGUILayout.LabelField("Attached components:", EditorStyles.centeredGreyMiniLabel);
        GUILayout.Space(3);
        window.pointerEffect = (GameObject)EditorGUILayout.ObjectField("Pointer Effect VFX", window.pointerEffect, typeof(GameObject));
        EditorGUILayout.LabelField("Main params:", EditorStyles.centeredGreyMiniLabel);

        window.MoveSpeed = EditorGUILayout.FloatField(new GUIContent("Move speed", "Скорость, с который будет двигаться персонаж"), window.MoveSpeed);
        window.AngularSpeed= EditorGUILayout.FloatField(new GUIContent("Angular speed", "Скорость поворота персонажа"), window.AngularSpeed);
        window.acceleration = EditorGUILayout.FloatField(new GUIContent("Acceleration", "хуй знает что это"), window.acceleration);
        window.stoppingDistance = EditorGUILayout.FloatField(new GUIContent("Stopping distance", "Это дистанция на которой будет останавливаться персонаж относительно цели движения"), window.stoppingDistance);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Skills:", Header, GUILayout.Height(20));
        GUILayout.Space(10);

        window.skillExamplePrefab = (GameObject)EditorGUILayout.ObjectField("Example skill prefab", window.skillExamplePrefab, typeof(GameObject));

        EditorGUILayout.LabelField("General params:", EditorStyles.centeredGreyMiniLabel);
        window.skillsColor = EditorGUILayout.ColorField("Skills main color", window.skillsColor);
        window.skillsCooldownColor = EditorGUILayout.ColorField("Skills cooldown color", window.skillsCooldownColor);
        window.skillsCooldownTextColor = EditorGUILayout.ColorField("Skills cooldown timer color", window.skillsCooldownTextColor);
        window.skillsFont = (Font)EditorGUILayout.ObjectField("Cooldown timer font", window.skillsFont, typeof(Font));
        window.fontSize = EditorGUILayout.IntField("Cooldown timer font size", window.fontSize);
        GUILayout.Space(5);

        if (!window.textIco)
            window.textIco = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/text.png", typeof(Texture2D));
        if (!window.paramIco)
            window.paramIco = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/Param.png", typeof(Texture2D));
        if (!window.cooldownIco)
            window.cooldownIco = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/Cooldown.png", typeof(Texture2D));
        EditorGUILayout.LabelField("Skills list:", EditorStyles.centeredGreyMiniLabel);

        if (window.Skills != null)
        {
            for (int i = 0; i < window.Skills.Count; i++)
            {
                GUILayout.Box("", GUILayout.Height(2), GUILayout.Width(Screen.width / 1.1f));
                EditorGUILayout.LabelField(window.Skills[i].SkillName, skillHeader);
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                window.Skills[i].icon = (Sprite)EditorGUILayout.ObjectField(window.Skills[i].icon, typeof(Sprite), false, GUILayout.Width(65f), GUILayout.Height(65f));
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Type of skill", GUILayout.Width(Screen.width / 3f));
                window.skilltype = EditorGUILayout.IntPopup(window.skilltype, new string[] { "Directed on target", "Directed at range", "Directed at heimself" }, new int[] { 0, 1, 2 }, GUILayout.Width(Screen.width / 2));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(window.textIco, GUILayout.Width(16), GUILayout.Height(16));
                window.Skills[i].SkillName = EditorGUILayout.TextField("Skill name: ", window.Skills[i].SkillName);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(window.paramIco, GUILayout.Width(16), GUILayout.Height(16));
                window.Skills[i].Param = EditorGUILayout.FloatField("Factor: ", window.Skills[i].Param);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(window.cooldownIco, GUILayout.Width(16), GUILayout.Height(16));
                window.Skills[i].Cooldown = EditorGUILayout.FloatField("Cooldown: ", window.Skills[i].Cooldown);
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("-", "Удалить скилл"), GUILayout.Width(Screen.width / 4)))
                    window.Skills.RemoveAt(i);
                window.Skills[i].InvokedVoid = EditorGUILayout.TextField("Invoke call: ", window.Skills[i].InvokedVoid);
                GUILayout.EndHorizontal();
                GUILayout.Box("", GUILayout.Height(2), GUILayout.Width(Screen.width / 1.1f));
            }
        }
        else window.Skills = new List<global::Skill>();
        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width / 2 - Screen.width / 6);
        if (GUILayout.Button(new GUIContent("+", "Создать новый скилл"), GUILayout.Width(Screen.width / 3)))
            window.Skills.Add(new Skill("Some Skill"));

        GUILayout.Space(15);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

    }

    internal void Skill()
    {

    }
}