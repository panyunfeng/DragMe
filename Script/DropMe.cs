using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropMe : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.LogError("abc");
    }
}
