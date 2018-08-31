using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class TheCamera : MonoBehaviour {

    Transform cam { get { return transform; } set { value = transform; } }
    Transform smTarget;

    public static TheCamera instance;

    [Header("Camera Settings")]
    public float speed;
  
    public Transform target;
    public Vector3 Offset;
    Controller controller;

    [Header("Border")]
    public Vector3 Center;
    public float XBorder, YBorder, Height = 3;
#if UNITY_EDITOR
    public Color BordersColor = Color.yellow;
#endif
    float x_min, y_min, x_max, y_max;

    float pointer_x, pointer_y;


    private void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        if (!target)
            target = Controller.instance.transform;
        smTarget = (new GameObject("smTarget")).transform;
        controller = FindObjectOfType<Controller>();

        cam.position -= new Vector3(0, Offset.z, 0);
        smTarget.position = cam.position;
    }

    void LateUpdate()
    {
        smTarget.position = new Vector3(target.position.x - Offset.x, smTarget.position.y, target.position.z - Offset.y);
        cam.position = Vector3.Lerp(cam.position, smTarget.position, speed * Time.deltaTime);
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = BordersColor;
        Gizmos.DrawWireCube(Center, new Vector3(XBorder, Height, YBorder));
    }
#endif
}
