using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainPhase1 : Enemy
{
    [SerializeField] GameObject Phase2;

    public override void OnDeath() {
        for(int i = 0; i < 2; i++){
            Enemy enemy = Instantiate(Phase2, new Vector2(transform.position.x + (0.3f * i), transform.position.y), Quaternion.identity).GetComponent<Enemy>();
            enemy.Init(Path);
            enemy.CurrentIndex = CurrentIndex;
            enemy.CurrentTarget = CurrentTarget;
            GameManager.EnemySpawned();
        }
        base.OnDeath();
    }
}
