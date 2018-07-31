using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class testDragCallBack : DragCallBackBase
{
    public int index = -1;
    public override void OnBeginDragCallBack(PointerEventData eventData)
    {
        Debug.LogError(index + "//" + eventData.pointerEnter.name);
    }

    public override void OnDragCallBack(PointerEventData eventData)
    {
        if (eventData != null && eventData.pointerEnter != null)
        {
            Debug.LogError(index + "//" + eventData.pointerEnter.name);
        }
        else
        {
            int i = 0;
        }
    }

    public override void OnEndDragCallBack(PointerEventData eventData)
    {
        if (eventData != null && eventData.pointerEnter != null)
        {
            Debug.LogError(index + "//" + eventData.pointerEnter.name);
        }
    }

    public override void OnDropCallBack(PointerEventData eventData)
    {
        Debug.LogError(index + "//" + eventData.pointerEnter.name);
    }
}
