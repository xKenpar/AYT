using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Enemy
{
    [SerializeField] float WormCloneTimer;
    float _timer;
    Vector2 _nextSpawnPosition;

    public override void Init(LineRenderer path) {
        base.Init(path);
        _timer = WormCloneTimer;
    }

    public override void Update() {
        base.Update();
        
        if(_timer >= 0f){
            _timer -= Time.deltaTime;
            if(_timer <= 0.5f && _timer >= 0.3f){
                _nextSpawnPosition = transform.position;
            }
        } 
        else{
            _timer = WormCloneTimer;
            Enemy enemy = Instantiate(this.gameObject, _nextSpawnPosition, Quaternion.identity).GetComponent<Enemy>();
            enemy.Init(Path);
            enemy.CurrentIndex = CurrentIndex;
            enemy.CurrentTarget = CurrentTarget;
            GameManager.EnemySpawned();
        }
    }
}
