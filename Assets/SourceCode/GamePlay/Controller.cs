using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditorInternal;

using Skills;

[RequireComponent(typeof(CharacterController))]
public class Controller : MonoBehaviour
{
    public enum typeOfSkill
    {
        directed, non_directed, directed_on_target
    }

    #region Params
    public CharacterController controller { get { return GetComponent<CharacterController>(); } set { value = GetComponent<CharacterController>(); } }
    public Animator animator;
    public TheCamera mainCamera;
    public GameplayUI GameplayUI;
    public Transform pointer;

    public List<Skill> skills;

    public float stopAnimDistance, speed, rotationSpeed, gravity;
    //for animations
    public float SmoothRotation;

    #endregion

    #region Varablies
    private RaycastHit hit;
    private Vector3 direction;
    private float dir_anim;

    [HideInInspector]
    public bool nonClick, usedTeleport, stop, actioned;

    [HideInInspector]
    public int skill = 0, animNum;
    #endregion

    #region Customisation window
    #if UNITY_EDITOR

    public int gridNum;
    public Transform Canvas;
    public GameObject skillExamplePrefab;
    public Color skillsColor, skillsCooldownColor, skillsCooldownTextColor;
    public Font skillsFont;
    public int fontSize, skilltype;
    public Texture2D textIco, paramIco, cooldownIco;

    public bool setControllerHide;

    public ReorderableList listSkills = null;

    #endif
    #endregion

    #region Constnats 
    const float maxAngle = 50f;
    const float minAxis = 0.14f;
    #endregion

    void Start()
    {
        mainCamera = FindObjectOfType<TheCamera>();
        GameplayUI = FindObjectOfType<GameplayUI>();
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float max = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(GameplayUI.InputVector.x, 2) + Mathf.Pow(GameplayUI.InputVector.z, 2)));
        controller.Move(-Vector3.up * gravity * Time.deltaTime);
        if (max > minAxis)
        {
            controller.Move(transform.forward * max * speed * Time.deltaTime);
            if (GameplayUI.dragged)
            {
                float angle = (float)(Math.Atan2(GameplayUI.InputVector.x, GameplayUI.InputVector.z) / Math.PI * 180);
                transform.eulerAngles = new Vector3(transform.localEulerAngles.x, Mathf.LerpAngle(transform.localEulerAngles.y, angle, rotationSpeed * Time.deltaTime), transform.localEulerAngles.x);
                dir_anim = Mathf.Lerp(dir_anim, GetNormalizedAngle(Mathf.DeltaAngle(transform.localEulerAngles.y, angle), maxAngle), SmoothRotation * Time.deltaTime);
            }
            else dir_anim = Mathf.Lerp(dir_anim, 0, SmoothRotation * Time.deltaTime);
            animator.SetFloat("Speed", max);
            animator.SetFloat("Direction", dir_anim);
        }
        else animator.SetFloat("Speed", 0);

    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(1))
            animator.CrossFade("Sword Attack Turn Right", 0.2f, 1);
    }

    public void SetSkill(int num)
    {
        skill = num;
    }

    public float GetNormalizedAngle(float value, float max)
    {
        return Math.Abs(value) > max ? Math.Sign(value) : (value / (max / 100)) / 100;
    }
}

