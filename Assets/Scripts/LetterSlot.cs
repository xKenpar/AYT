using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LetterSlot : MonoBehaviour, IDropHandler {
    void IDropHandler.OnDrop(PointerEventData eventData) {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null) {
            eventData.pointerDrag.GetComponent<Transform>().position = GetComponent<Transform>().position;
        }
    }
}
