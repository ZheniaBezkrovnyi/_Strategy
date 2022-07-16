using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Touch : MonoBehaviour,IDragHandler, IEndDragHandler
{
    public bool Drag { get { return drag; } }
    public bool drag;
    public bool startMove;
    public bool endMove;
    public StateHouse stateHouse;
    public void OnDrag(PointerEventData eventData)
    {
        drag = true;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        stateHouse = StateHouse.IsActive;
        drag =  false;
    }
}
