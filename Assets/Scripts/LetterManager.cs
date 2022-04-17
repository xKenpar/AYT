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

    [SerializeField] List<GameObject> LetterPrefabs;

    [SerializeField] List<Sprite> PreviewSprites;
    [SerializeField] SpriteRenderer PreviewRenderer;

    [SerializeField] Transform PreviewBar;

    [SerializeField] BoxCollider2D Deck;

    [SerializeField] List<GameObject> RecycleBars; 
    int _recycleProgress = 0;

    [SerializeField] GameObject NewSlotEffect;

    [SerializeField] List<GameObject> LockedSlots;
    [SerializeField] List<GameObject> LockIcons;
    int _currentSlot = 0;


    LetterEnum _currentPreview;
    float _previewProgress = 0;

    public static LetterManager Instace{get;private set;}

    void Awake() {
        Instace = this;

        foreach(var bar in RecycleBars)
            bar.SetActive(false);
        PreviewBar.localScale = new Vector3(_previewProgress, 1, 1);

        _currentPreview = (LetterEnum)Random.Range(0,15);
        PreviewRenderer.sprite = PreviewSprites[(int)_currentPreview];

        for(int i = 0;i < 3;i++)
            SpawnNewLetter((LetterEnum)Random.Range(0,15));
    }

    public static void EnemyKilled() {
        Instace._previewProgress += .2f;
        if(Instace._previewProgress >= 1){
            Instace._previewProgress = 0;

            Instace.SpawnNewLetter(Instace._currentPreview);
            Instace._currentPreview = (LetterEnum)Random.Range(0,15);
            Instace.PreviewRenderer.sprite = Instace.PreviewSprites[(int)Instace._currentPreview];
        }

        Instace.PreviewBar.localScale = new Vector3(Instace._previewProgress, 1, 1);
        Instace.PreviewBar.localPosition = new Vector3((1-Instace._previewProgress)/2,0);
    }

    void SpawnNewLetter(LetterEnum newLetter) {
        Instantiate(LetterPrefabs[(int)newLetter] ,
                    new Vector3(Random.Range(Deck.bounds.min.x+.3f, Deck.bounds.max.x-.3f), Random.Range(Deck.bounds.min.y+.3f, Deck.bounds.max.y-.3f)), Quaternion.identity);
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
        LockIcons[_currentSlot].SetActive(false);
        LockedSlots[_currentSlot].SetActive(true);
        Instantiate(NewSlotEffect, LockedSlots[_currentSlot].transform.position, Quaternion.identity);
        _currentSlot++;
    }
}
