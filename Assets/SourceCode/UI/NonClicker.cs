using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NonClicker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    Controller controller;

    private void Awake()
    {
        controller = FindObjectOfType<Controller>();
    }


    public void OnPointerEnter(PointerEventData data)
    {
        controller.nonClick = true;
    }

    public void OnPointerExit(PointerEventData data)
    {
        controller.nonClick = false;
    }
}
