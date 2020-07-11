﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class GenericDragAndDropEvents : UnityEvent<PointerEventData> { }

[System.Serializable]
public class GDDE_Enter : GenericDragAndDropEvents { }
[System.Serializable]
public class GDDE_Exit : GenericDragAndDropEvents { }
[System.Serializable]
public class GDDE_PointerDown : GenericDragAndDropEvents { }
[System.Serializable]
public class GDDE_PointerUp : GenericDragAndDropEvents { }

public class GenericDragAndDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Transform targetTransform;
    public bool interactable = true;
    public bool dragWithLeft = true;
    public bool pointerIsOver = false;
    public bool pointerIsDown = false;
    public bool dragging = false;
    public Vector3 pointerClickLocation = Vector3.zero;
    private GameObject dragPointObj;
    public GDDE_Enter pointerEntered;
    public GDDE_Exit pointerExited;
    public GDDE_PointerDown pointerDown;
    public GDDE_PointerDown pointerDoubleClick;
    public GDDE_PointerDown rightClick;
    public GDDE_PointerUp pointerUp;

    void Start()
    {
        if (targetTransform == null)
        {
            targetTransform = transform;
        }
    }
    Transform DragPoint
    {
        get
        {
            if (dragPointObj == null)
            {
                dragPointObj = new GameObject(gameObject.name + "DragPoint");
            }
            return dragPointObj.transform;
        }
    }
    public void OnPointerEnter(PointerEventData coll)
    {
        //   Debug.Log("Pointer enter" + coll);
        pointerIsOver = true;
        pointerDown.Invoke(coll);
    }
    public void OnPointerExit(PointerEventData coll)
    {
        //  Debug.Log("Pointer exit " + coll);
        pointerIsOver = false;
        pointerExited.Invoke(coll);
    }
    public void OnPointerDown(PointerEventData coll)
    {
        //Debug.Log("Pointer down" + coll);
        pointerClickLocation = Camera.main.ScreenToWorldPoint(coll.position);
        if (coll.button == PointerEventData.InputButton.Left && dragWithLeft)
        {
            pointerIsDown = true;
            pointerDown.Invoke(coll);
        }
        else if (dragWithLeft && coll.button != PointerEventData.InputButton.Left)
        {
            Debug.Log("Right clicked");
            rightClick.Invoke(coll);
        }
        else
        {
            pointerIsDown = true;
            pointerDown.Invoke(coll);
        }
        Debug.Log(coll.clickCount);
        if (coll.clickCount > 1)
        {
            pointerDoubleClick.Invoke(coll);
        }
    }
    public void OnPointerUp(PointerEventData coll)
    {
        // Debug.Log("Pointer up" + coll);
        if (dragWithLeft && coll.button == PointerEventData.InputButton.Left)
        {
            pointerClickLocation = Vector3.zero;
            pointerIsDown = false;
            pointerUp.Invoke(coll);
        };
    }

    void Update()
    {
        if (interactable)
        {
            if (pointerIsOver && pointerIsDown)
            {
                if (!dragging)
                {
                    DragPoint.position = pointerClickLocation;
                    targetTransform.SetParent(DragPoint, true);
                    dragging = true;
                }
                else
                {
                    DragPoint.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                if (dragging && !pointerIsDown)
                {
                    targetTransform.SetParent(null, true);
                    dragging = false;
                };
            }
        }
        else
        {
            targetTransform.SetParent(null, true);
            dragging = false;
        }
    }

}