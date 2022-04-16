using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}

    WaveManager _waveManager;

    int _enemyCount;
    bool _waveActive = false;
    int _currentWave = 0;
    
    void Awake() {
        Instance = this;
        _waveManager = GetComponent<WaveManager>();
    }

    void Start(){
        Invoke(nameof(NextWave),5f);
    }

    public static void EnemySpawned() {
        Instance._enemyCount++;
    }

    public static void EnemyDied() {
        if(--Instance._enemyCount <= 0 && !Instance._waveActive)
            Instance.NextWave();
    }

    public static void WaveEnded() {
        Instance._waveActive = false;
        if(Instance._enemyCount <= 0)
            Instance.NextWave();
    }

    public void NextWave() {
        _waveActive = true;
        StartCoroutine(_waveManager.SpawnWave(_currentWave++));
    }
}
