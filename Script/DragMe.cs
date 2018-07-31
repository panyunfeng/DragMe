using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragMe : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    #region Delegate
    public class DragDelegateMgr
    {
        public enum DragDelegateType
        {
            OnBeginDrag = 0,
            OnDrop = 1,
        }
        public delegate void VoidDelegate();
        private VoidDelegate OnBeginDrag;
        private VoidDelegate OnDrop;

        public void AddDelegate(DragDelegateType type, VoidDelegate _delegate)
        {
            switch (type)
            {
                case DragDelegateType.OnBeginDrag:
                    OnBeginDrag += _delegate;
                    break;
                case DragDelegateType.OnDrop:
                    OnDrop += _delegate;
                    break;
                default:
                    break;
            }
        }
        public void ReduceDelegate(DragDelegateType type, VoidDelegate _delegate)
        {
            switch (type)
            {
                case DragDelegateType.OnBeginDrag:
                    OnBeginDrag -= _delegate;
                    break;
                case DragDelegateType.OnDrop:
                    OnDrop -= _delegate;
                    break;
                default:
                    break;
            }
        }

        public void OnBeginDragDelegate()
        {
            if (OnBeginDrag != null)
                OnBeginDrag();
        }

        public void OnDropDelegate()
        {
            if (OnDrop != null)
            {
                OnDrop();
            }
        }
    }
    private DragDelegateMgr dargDelegate;
    public DragDelegateMgr m_DargDelegate
    {
        get
        {
            if (dargDelegate == null)
            {
                dargDelegate = new DragDelegateMgr();
            }
            return dargDelegate;
        }
    }
    #endregion
    public DragCallBackBase dragCallBack;
    public enum ActionType
    {
        Both = 0,
        Drag = 1,
        Drop = 2,
    }

    public ActionType acitonType = ActionType.Both;
    [SerializeField, TooltipAttribute("ActionType为Drop时该属性无效")]
    public bool dragOnSurface = true;
    public List<Image> dragChildren = new List<Image>();

    private GameObject m_DraggingObj;
    private RectTransform m_DraggingPlane;
    private Image _image;

    public Image M_Image
    {
        get
        {
            if (_image == null)
            {
                _image = transform.GetComponent<Image>();
            }
            return _image;
        }
    }

    public void SetDargCallBack(DragCallBackBase callBackBase)
    { }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanDrag())
            return;
        Canvas canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;

        // We have clicked something that can be dragged.
        // What we want to do is create an object for this.
        m_DraggingObj = new GameObject("Drag");

        m_DraggingObj.transform.SetParent(canvas.transform, false);
        m_DraggingObj.transform.SetAsLastSibling();

        var image = m_DraggingObj.AddComponent<Image>();

        image.sprite = M_Image.sprite;

        CanvasGroup canvasGroup = m_DraggingObj.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        if (dragOnSurface)
            m_DraggingPlane = transform as RectTransform;
        else
            m_DraggingPlane = canvas.transform as RectTransform;
        
        //deal with the children 
        for (int i = 0; i < dragChildren.Count; i++)
        {
            SetChildren(dragChildren[i], dragChildren[i].name, m_DraggingObj);
        }

        SetDraggedPosition(eventData);

        if (m_DargDelegate != null)
        {
            m_DargDelegate.OnBeginDragDelegate();
        }
        if (dragCallBack != null && eventData != null)
        {
            dragCallBack.OnBeginDragCallBack(eventData);
        }
    }

    public void SetChildren(Image img, string name, GameObject parent)
    {
        if (img == null)
            return;
        GameObject obj = new GameObject(name);
        Image image = obj.AddComponent<Image>();
        image.sprite = img.sprite;
        obj.transform.SetParent(parent.transform);
        obj.transform.localPosition = img.transform.localPosition;
        obj.transform.localRotation = img.transform.localRotation;
        obj.transform.localScale = img.transform.localScale;
    }
    private void SetChildren(Image originImg, Image targetImg)
    {
        targetImg.sprite = originImg.sprite;
        targetImg.transform.localPosition = originImg.transform.localPosition;
        targetImg.transform.localScale = originImg.transform.localScale;
        targetImg.transform.localRotation = originImg.transform.localRotation;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag())
            return;
        if (m_DraggingObj != null)
            SetDraggedPosition(eventData);
        if (dragCallBack != null && eventData != null)
        {
            dragCallBack.OnDragCallBack(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CanDrag())
            return;
        if (m_DraggingObj != null)
            Destroy(m_DraggingObj);
        if (dragCallBack != null && eventData != null)
        {
            dragCallBack.OnEndDragCallBack(eventData);
        }
    }
    private void SetDraggedPosition(PointerEventData data)
    {
        if (dragOnSurface && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            m_DraggingPlane = data.pointerEnter.transform as RectTransform;

        var rt = m_DraggingObj.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlane.rotation;
        }
    }
    public static T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!CanDrop())
            return;
        if (eventData == null || eventData.pointerDrag == null)
            return;
        DragMe originDragMe = eventData.pointerDrag.GetComponent<DragMe>();
        
        if (originDragMe == null)
        {
            return;
        }

        M_Image.sprite = originDragMe.M_Image.sprite;

        if(dragChildren.Count>0 && originDragMe.dragChildren.Count >0)
        {
            for (int i = 0; i < originDragMe.dragChildren.Count; i++)
            {
                if (dragChildren.Count > i)
                {
                    SetChildren(originDragMe.dragChildren[i], dragChildren[i]);
                }
            }
        }

        if (m_DargDelegate != null)
        {
            m_DargDelegate.OnDropDelegate();
        }
        if (dragCallBack != null && eventData != null)
        {
            dragCallBack.OnDropCallBack(eventData);
        }
    }



    private bool CanDrag()
    {
        return acitonType == ActionType.Both || acitonType == ActionType.Drag;
    }

    private bool CanDrop()
    {
        return acitonType == ActionType.Both || acitonType == ActionType.Drop;
    }
}
