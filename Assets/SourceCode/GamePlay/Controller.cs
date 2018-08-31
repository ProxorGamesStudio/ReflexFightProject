using System;
using System.Collections;
using System.Collections.Generic;
//Unity
using UnityEngine;
using UnityEngine.AI;
using UnityEditorInternal;
//Custom
using Skills;
using ProxorServer;

[RequireComponent(typeof(CharacterController))]
public class Controller : MonoBehaviour
{

    #region Params
    public Animator animator;
    public List<Skill> skills;
    public float speed;
    public float rotationSpeed;
    public float gravity; 
    //for animations
    public float SmoothRotation;
    #endregion

    #region Varablies
    private Vector3 direction;
    private float currentAnim;

    private SurfaceDetectionComponent SurfaceDetectionComponent;
    private TheCamera mainCamera;
    private GameplayUI GameplayUI;
    private CharacterController controller;

    [HideInInspector]
    public bool nonClick;

    public static Controller instance;
    #endregion

    #region Customisation window
    #if UNITY_EDITOR

    public Transform Canvas;
    public Texture2D textIco, paramIco, cooldownIco;
    public ReorderableList listSkills = null;
    #endif

    #endregion

    #region Constnats 
    const float maxAngle = 50f;
    const float minAxis = 0.14f;

    #endregion

    #region Network
    [Synchronizable]
    public Vector3 nPosition;
    [Synchronizable]
    public Quaternion nRotation;
    #endregion

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        SurfaceDetectionComponent = GetComponentInChildren<SurfaceDetectionComponent>();
        mainCamera = TheCamera.instance;
        GameplayUI = GameplayUI.instance;
    }

    private void Update()
    {
        Movement();
        HandleInformation();
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
                currentAnim = Mathf.Lerp(currentAnim, GetNormalizedAngle(Mathf.DeltaAngle(transform.localEulerAngles.y, angle), maxAngle), SmoothRotation * Time.deltaTime);
            }
            else currentAnim = Mathf.Lerp(currentAnim, 0, SmoothRotation * Time.deltaTime);
            animator.SetFloat("Speed", max);
            animator.SetFloat("Direction", currentAnim);
        }
        else animator.SetFloat("Speed", 0);

    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(1))
            animator.CrossFade("Sword Attack Turn Right", 0.2f, 1);
    }

    public float GetNormalizedAngle(float value, float max)
    {
        return Math.Abs(value) > max ? Math.Sign(value) : (value / (max / 100)) / 100;
    }

    #region Network
    public void HandleInformation()
    {
        nPosition = transform.position;
        nRotation = transform.rotation;
    }
    #endregion
}

