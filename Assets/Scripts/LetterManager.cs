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

    public static LetterManager Instance{get;private set;}

    void Awake() {
        Instance = this;

        foreach(var bar in RecycleBars)
            bar.SetActive(false);
        PreviewBar.localScale = new Vector3(_previewProgress, 1, 1);

        _currentPreview = (LetterEnum)Random.Range(0,15);
        PreviewRenderer.sprite = PreviewSprites[(int)_currentPreview];

        for(int i = 0;i < 30;i++)
            SpawnNewLetter((LetterEnum)Random.Range(0,15));
    }

    public static void EnemyKilled() {
        Instance._previewProgress += .2f;
        if(Instance._previewProgress >= 1){
            Instance._previewProgress = 0;

            Instance.SpawnNewLetter(Instance._currentPreview);
            Instance._currentPreview = (LetterEnum)Random.Range(0,15);
            Instance.PreviewRenderer.sprite = Instance.PreviewSprites[(int)Instance._currentPreview];
        }

        Instance.PreviewBar.localScale = new Vector3(Instance._previewProgress, 1, 1);
        Instance.PreviewBar.localPosition = new Vector3((1-Instance._previewProgress)/2,0);
    }

    void SpawnNewLetter(LetterEnum newLetter) {
        Instantiate(LetterPrefabs[(int)newLetter] ,
                    new Vector3(Random.Range(Deck.bounds.min.x+.3f, Deck.bounds.max.x-.3f), Random.Range(Deck.bounds.min.y+.3f, Deck.bounds.max.y-.3f)), Quaternion.identity);
    }

    public static void RecycleLetter() {
        AudioManager.Play(AudioType.Trash);

        Instance._recycleProgress++;
        if(Instance._recycleProgress >= Instance.RecycleBars.Count){
            foreach(var bar in Instance.RecycleBars)
                bar.SetActive(false);
            Instance._recycleProgress = 0;
            Instance.OpenNewSlot();    
        } else {
            Instance.RecycleBars[Instance._recycleProgress-1].SetActive(true);
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
