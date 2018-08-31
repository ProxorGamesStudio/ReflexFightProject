using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class gTreeHideComponent : MonoBehaviour
{
    public string propertyName;

    private void Update()
    {
        Shader.SetGlobalVector(propertyName, transform.position);
    }
}
