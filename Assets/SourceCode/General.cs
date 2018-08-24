using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
#region Editor 

public interface DrawComponent
{
    void Draw<T>(T _window);
}

#endregion
#endif