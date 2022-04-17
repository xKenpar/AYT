using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainPhase2 : Enemy
{
    [SerializeField] GameObject Phase3;

    public override void OnDeath() {
        for(int i = 0; i < 3; i++){
            Enemy enemy = Instantiate(Phase3, new Vector2(transform.position.x + (0.2f * i), transform.position.y), Quaternion.identity).GetComponent<Enemy>();
            enemy.Init(Path);
            enemy.CurrentIndex = CurrentIndex;
            enemy.CurrentTarget = CurrentTarget;
            GameManager.EnemySpawned();
        }
        base.OnDeath();
    }
}
