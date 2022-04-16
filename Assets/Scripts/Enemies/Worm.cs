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
            if(_timer <= 1f && _timer >= 0.8f){
                _nextSpawnPosition = transform.position;
            }
        } 
        else{
            _timer = WormCloneTimer;
            Instantiate(this.gameObject, _nextSpawnPosition, Quaternion.identity).GetComponent<Enemy>().Init(Path);
        }
    }
}
