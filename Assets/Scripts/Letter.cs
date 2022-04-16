using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Letter : MonoBehaviour
{
    public Transform assignedSlot = null;
    public bool draggable = true;

    public void MoveBack(Vector3 newPos) {
        draggable = false;
        transform.DOMove(newPos, 1)
            .SetEase(Ease.OutQuint)
            .OnComplete(() => draggable = true);
    }
}
