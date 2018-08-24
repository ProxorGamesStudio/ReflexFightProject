using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using Skills;

[CustomEditor(typeof(Controller))]
[System.Serializable]
public partial class ControllerBrowser : Editor
{

    private DrawComponent[] DrawComponents = new DrawComponent[] { new AttachedComponents(), new MainProperties(), new AnimationSettings() };
    Controller window;

    private void OnEnable()
    {
        window = target as Controller;

        if (window.listSkills == null)
        {
            window.listSkills = new ReorderableList(window.skills, typeof(Skill), true, true, true, true);
            window.listSkills.elementHeight = 16;

            window.listSkills.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                try
                {
                    window.skills[index] = (Skill)EditorGUI.ObjectField(rect, window.skills[index].name, window.skills[index], typeof(Skill), false);
                }
                catch
                {
                    window.skills[index] = (Skill)EditorGUI.ObjectField(rect, "Emty Skill", window.skills[index], typeof(Skill), false);
                }
                    
            };

            window.listSkills.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Player skills:");
            };
            window.listSkills.onAddCallback = (list) =>
            {
                window.skills.Add(null);
            };

            window.listSkills.onRemoveCallback = (list) =>
            {
                window.skills.RemoveAt(list.index);
            };
        }
    }

    public override void OnInspectorGUI()
    {
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
        EditorGUILayout.LabelField("Player Settings:", Header, GUILayout.Height(20));

        foreach (DrawComponent dc in DrawComponents)
            dc.Draw(window);

        EditorGUILayout.LabelField("Skills", EditorStyles.centeredGreyMiniLabel);

        if (window.listSkills != null)
            window.listSkills.DoLayoutList();
    }
}

#region Drawing 


public partial class ControllerBrowser
{

    private class AttachedComponents : DrawComponent
    {

        public void Draw<T>(T _window)
        {
            var window = _window as Controller;

            EditorGUILayout.LabelField("Attached components:", EditorStyles.centeredGreyMiniLabel);
            GUILayout.Space(3);

            GUILayout.BeginHorizontal();
            GUILayout.Label(window.paramIco, GUILayout.Width(16), GUILayout.Height(16));
            window.animator = (Animator)EditorGUILayout.ObjectField("Animator", window.animator, typeof(Animator));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(window.paramIco, GUILayout.Width(16), GUILayout.Height(16));
            window.Canvas = (Transform)EditorGUILayout.ObjectField(new GUIContent("Canvas rect", "Элемент канваса куда будут писать и откуда будут браться скиллы"), window.Canvas, typeof(Transform));
            GUILayout.EndHorizontal();

            GUILayout.Box("", GUILayout.Height(2), GUILayout.Width(Screen.width / 1.1f));
        }
    }

    private class MainProperties : DrawComponent
    {
        public void Draw<T>(T _window)
        {
            var window = _window as Controller;

            EditorGUILayout.LabelField("Main Properties:", EditorStyles.centeredGreyMiniLabel);

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Label(window.paramIco, GUILayout.Width(16), GUILayout.Height(16));
            window.speed = EditorGUILayout.FloatField(new GUIContent("Move speed", "Скорость, с который будет двигаться персонаж"), window.speed);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(window.paramIco, GUILayout.Width(16), GUILayout.Height(16));
            window.rotationSpeed = EditorGUILayout.FloatField(new GUIContent("Rotation Speed", "Скорость, с который будет поворачиваться персонаж"), window.rotationSpeed);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(window.paramIco, GUILayout.Width(16), GUILayout.Height(16));
            window.gravity = EditorGUILayout.FloatField(new GUIContent("Gravitation", "Гравитация"), window.gravity);
            GUILayout.EndHorizontal();

            GUILayout.Box("", GUILayout.Height(2), GUILayout.Width(Screen.width / 1.1f));
        }
    }

    private class AnimationSettings : DrawComponent
    {
        public void Draw<T>(T _window)
        {
            var window = _window as Controller;

            EditorGUILayout.LabelField("Animation settings", EditorStyles.centeredGreyMiniLabel);

            GUILayout.BeginHorizontal();
            GUILayout.Label(window.paramIco, GUILayout.Width(16), GUILayout.Height(16));
            window.SmoothRotation = EditorGUILayout.FloatField(new GUIContent("Rotation smooth", "Сглаживание анимации поворта"), window.SmoothRotation);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            /*if (window.gridNum == 1)
            {
                window.profile = (Profiles.ControllerProfile)EditorGUILayout.ObjectField("Profile", window.profile, typeof(Profiles.ControllerProfile));
                if (GUILayout.Button(new GUIContent("Copy from Profile", "Скопировать настройки из профиля"), GUILayout.Width(Screen.width / 4)) && window.profile)
                {
                    window.speed = window.profile.MoveSpeed;
                    window.stopAnimDistance = window.profile.stopAnimDistance;
                    window.skills = window.profile.Skills;
                }



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
                    window.speed = window.profile.MoveSpeed;   
            }
        }*/
        }
    }


}



#endregion