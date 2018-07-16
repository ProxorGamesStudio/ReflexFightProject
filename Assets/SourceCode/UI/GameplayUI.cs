using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameplayUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    public Image JoistickBackground, JoystickCircle;

    private Vector3 _inputVector;
    public Vector3 InputVector
    {
        get
        {
            return _inputVector;
        }
    }
    [HideInInspector]
    public bool dragged;

    public void OnPointerDown(PointerEventData e)
    {
        OnDrag(e);
        dragged = true;
    }

    public void OnDrag(PointerEventData e)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(JoistickBackground.rectTransform, e.position, e.pressEventCamera, out pos))
        {

            pos.x = (pos.x / JoistickBackground.rectTransform.sizeDelta.x);
            pos.y = (pos.y / JoistickBackground.rectTransform.sizeDelta.y);

            _inputVector = new Vector3(pos.x * 2.5f, 0, pos.y * 2.5f);
            _inputVector = (_inputVector.magnitude > 1.0f) ? _inputVector.normalized : _inputVector;

            JoystickCircle.rectTransform.anchoredPosition = new Vector3(_inputVector.x * (JoistickBackground.rectTransform.sizeDelta.x / 3),
                                                                     _inputVector.z * (JoistickBackground.rectTransform.sizeDelta.y / 3));
        }
    }

    public void OnPointerUp(PointerEventData e)
    {
        dragged = false;
    }


    public float Horizontal()
    {
        if (_inputVector.x != 0)
        {
            return _inputVector.x;
        }

        return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        if (_inputVector.z != 0)
        {
            return _inputVector.z;
        }

        return Input.GetAxis("Vertical");
    }

    private void Update()
    {
        if (!dragged)
        {
            JoystickCircle.rectTransform.anchoredPosition = Vector3.Lerp(JoystickCircle.rectTransform.anchoredPosition, Vector3.zero, 10 * Time.deltaTime);
            _inputVector = Vector3.Lerp(_inputVector, Vector3.zero, 6.5f * Time.deltaTime); 
        }
    }

   
}
