using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class TheCamera : MonoBehaviour {
    
    public enum mode
    {
        Free, Targeted, Hybrid
    }

    [Header("Camera Settings")]
    public mode Mode;
    Transform cam { get { return transform; } set { value = transform; } }
    Transform smTarget;
    public float speed, sensivity, moveDelta;
    Controller controller;
    Vector3 oldPos;
    public Transform target;
    public Vector3 Offset;
    bool toFree;
    
    [Header("Border")]
    public Vector3 Center;
    public float XBorder, YBorder, Height = 3;
#if UNITY_EDITOR
    public Color BordersColor = Color.yellow;
#endif
    float x_min, y_min, x_max, y_max;

    float pointer_x, pointer_y;


    void Start () {
        if (!target)
            target = GameObject.FindGameObjectWithTag("Player").transform;
        smTarget = (new GameObject("smTarget")).transform;
        controller = FindObjectOfType<Controller>();
        x_min = Center.x - XBorder/2;
        x_max = Center.x + XBorder/2;
        y_min = Center.z - YBorder/2;
        y_max = Center.z + YBorder/2;
        cam.position -= new Vector3(0, Offset.z, 0);
        smTarget.position = cam.position;
    }

    void LateUpdate() 
    {
        switch (Mode)
        {
            case mode.Free:
                pointer_x = Input.GetAxis("Mouse X");
                pointer_y = Input.GetAxis("Mouse Y");
                if (Input.touchCount > 0)
                {
                    pointer_x = Input.touches[0].deltaPosition.x / 15;
                    pointer_y = Input.touches[0].deltaPosition.y / 15;
                }

                if (Input.GetMouseButton(0) && checkBorder())
                {
                    Vector2 currecntPos = Input.mousePosition;
                    smTarget.position -= new Vector3(pointer_x, 0, pointer_y) * sensivity * Time.deltaTime;
                }
                else
                {
                    oldPos = transform.position;
                }
                break;

            case mode.Targeted:
                smTarget.position = new Vector3(target.position.x - Offset.x, smTarget.position.y, target.position.z - Offset.y);
                break;

            case mode.Hybrid:
                const float deltaOffset = 0f;
                pointer_x = Input.GetAxis("Mouse X");
                pointer_y = Input.GetAxis("Mouse Y");
                if (Input.touchCount > 0)
                {
                    pointer_x = Input.touches[0].deltaPosition.x / 15;
                    pointer_y = Input.touches[0].deltaPosition.y / 15;
                }
                if (Input.GetMouseButton(0) && (pointer_x > deltaOffset || pointer_x < deltaOffset || pointer_y > deltaOffset || pointer_y < deltaOffset))
                    toFree = true;

                if (toFree)
                {
                    if (Input.GetMouseButton(0) && checkBorder())
                    {
                        Vector2 currecntPos = Input.mousePosition;
                        smTarget.position -= new Vector3(pointer_x, 0, pointer_y) * sensivity * Time.deltaTime;
                    }
                    else
                    {
                        oldPos = transform.position;
                    }

                    if (!Input.GetMouseButton(0) && pointer_x == 0 && pointer_y == 0 && controller.stop && controller.actioned)
                        toFree = controller.actioned = false;
                }
                else
                {
                    smTarget.position = new Vector3(target.position.x - Offset.x, smTarget.position.y, target.position.z - Offset.y);
                }
                break;
        }
        cam.position = Vector3.Lerp(cam.position, smTarget.position, speed * Time.deltaTime);
    }

    public bool checkBorder()
    {
        return (cam.position.x > x_min || pointer_x < 0) && (cam.position.x < x_max || pointer_x > 0) && (cam.position.z < y_max || pointer_y > 0) && (cam.position.z > y_min || pointer_y < 0);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = BordersColor;
        Gizmos.DrawWireCube(Center, new Vector3(XBorder, Height, YBorder));
    }
#endif
}
