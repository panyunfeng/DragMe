using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCallBackBase
{
    public virtual void OnBeginDragCallBack(PointerEventData eventData)
    {}

    //慎用 多判断空指针
    public virtual void OnDragCallBack(PointerEventData eventData)
    {}
    
    public virtual void OnEndDragCallBack(PointerEventData eventData)
    {}

    public virtual void OnDropCallBack(PointerEventData eventData)
    {}

}
