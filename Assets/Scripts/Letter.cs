using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Letter : MonoBehaviour
{
    public Transform assignedSlot = null;
    public bool draggable = true;

    public void MoveBack(Vector3 newPos, float t = 1) {
        draggable = false;
        transform.DOMove(newPos, t)
            .SetEase(Ease.OutQuint)
            .OnComplete(() => draggable = true);
    }
}
