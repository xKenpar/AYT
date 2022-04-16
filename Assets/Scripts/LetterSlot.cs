using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSlot : MonoBehaviour 
{
    [SerializeField] Sprite spriteEmpty;
    [SerializeField] Sprite spriteAssigned;
    [SerializeField] Sprite spriteDuplicated;
    [HideInInspector] public Transform assignedLetter = null;

    SpriteRenderer _spriteRenderer;

    void Start() {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void AssignLetter(Transform newLetter) {
        assignedLetter = newLetter;
        _spriteRenderer.sprite = spriteAssigned;
    }

    public void DeassignLetter() {
        assignedLetter = null;
        _spriteRenderer.sprite = spriteEmpty;
    }
}
