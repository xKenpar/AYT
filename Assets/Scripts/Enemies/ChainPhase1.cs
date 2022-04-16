using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainPhase1 : Enemy
{
    [SerializeField] GameObject Phase2;

    public override void OnDeath() {
        for(int i = 0; i < 2; i++){
            Instantiate(Phase2, new Vector2(transform.position.x + (0.1f * i), transform.position.y), Quaternion.identity);
        }
        base.OnDeath();
    }
}
