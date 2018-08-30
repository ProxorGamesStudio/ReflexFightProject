using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceDetectionComponent : MonoBehaviour
{
    [SerializeField]
    private LayerMask LayerMask;
    [SerializeField]
    private float maxDistance = 20;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private float[] locomationModes;

    private RaycastHit hit;
    private Vector3 hitPoint;


    private void Awake()
    {
        hitPoint = transform.position;       
    }

    public void DoStep(float mode)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance, LayerMask) && mode == locomationModes[CheckLocomationMode(locomationModes, Controller.instance.speed)])
        {
            try
            {
                hitPoint = hit.point;
                hit.transform.GetComponent<ISurfaceHitInfo>().GetSurfaceProperties(hitPoint).Step_PlayHitEffect(audioSource);
            }
            catch
            {
                Debug.LogWarning("This collider has not ISurfaceHitInfo component!");
            }
        }
    }

    private static int CheckLocomationMode(float[] locomationModes, float value)
    {
        for (int i = 0; i < locomationModes.Length; i++)
            if (value <= locomationModes[i])
                return i;
        return 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, hitPoint);
    }
}

internal interface ISurfaceHitInfo
{
    SurfaceProperties GetSurfaceProperties(Vector3 castPos);
}