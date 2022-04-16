using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Letter : MonoBehaviour
{
    [HideInInspector] public Transform assignedSlot = null;
    [HideInInspector] public bool draggable = true;

    public string letterID = "A";
    Animator _animator;

    void Awake() {
        _animator = GetComponentInChildren<Animator>();
    }

    public void MoveTo(Vector3 newPos, float t = 1) {
        UpdateAnimationState(false);
        draggable = false;
        transform.DOMove(newPos, t)
            .SetEase(Ease.InCubic)
            .OnComplete(() => { draggable = true; UpdateAnimationState(true); }) ;
    }

    public void UpdateCycleOffset() {
        
    }

    public void UpdateAnimationState(bool newState) {
        _animator.speed = newState ? 1 : 0;
    }
}
