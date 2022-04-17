using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterManager : MonoBehaviour
{
    public enum LetterEnum {
        A, I,
        Z, F,
        B, Y,
        Question, Exclamation,
        O, U,
        S,
        Eight,
        G,
        P,
        Dash,
    }

    [SerializeField] List<GameObject> RecycleBars; 
    int _recycleProgress = 0;

    [SerializeField] GameObject NewSlotEffect;

    [SerializeField] List<GameObject> LockedSlots;
    int _currentSlot = 0;

    [SerializeField] List<Sprite> PreviewSprites;
    [SerializeField] SpriteRenderer PreviewRenderer;

    [SerializeField] Transform PreviewBar;

    LetterEnum _currentPreview;
    float _previewProgress = 0;

    void Awake() {
        foreach(var bar in RecycleBars)
            bar.SetActive(false);
        PreviewBar.localScale = new Vector3(_previewProgress, 1, 1);
    }

    public void RecycleLetter() {
        _recycleProgress++;
        if(_recycleProgress > RecycleBars.Count){
            foreach(var bar in RecycleBars)
                bar.SetActive(false);
            _recycleProgress = 0;
            OpenNewSlot();    
        } else {
            RecycleBars[_recycleProgress-1].SetActive(true);
        }
    }

    void OpenNewSlot() {
        if(_currentSlot >= LockedSlots.Count)
            return;
        LockedSlots[_currentSlot++].SetActive(true);
        Instantiate(NewSlotEffect, LockedSlots[_currentSlot-1].transform.position, Quaternion.identity);
    }

    public void EnemyKilled() {
        _previewProgress += .2f;
        if(_previewProgress >= 1){
            _previewProgress = 0;

            GetNewLetter(_currentPreview);
            _currentPreview = (LetterEnum)Random.Range(0,15);
            PreviewRenderer.sprite = PreviewSprites[(int)_currentPreview];
        }

        PreviewBar.localScale = new Vector3(_previewProgress, 1, 1);
        PreviewBar.localPosition = new Vector3((1-_previewProgress)/2,0);
    }

    void GetNewLetter(LetterEnum newLetter) {

    }
}
