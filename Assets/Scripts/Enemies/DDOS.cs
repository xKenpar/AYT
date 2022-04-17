using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOS : Enemy
{
    [SerializeField] GameObject self;
    [SerializeField] int MaxClones;
    [SerializeField] int MinClones;

    public bool IsHead = true;
    int i = 0;

    public override void Init(LineRenderer path) {
        base.Init(path);
        if(IsHead){
            //SPAWN
            StartCoroutine(SpawnDDOSes(Random.Range(MinClones, MaxClones)));
        }
    }

    IEnumerator SpawnDDOSes(int maxDDOS){
        while(i < maxDDOS){
            yield return new WaitForSeconds(.3f);
            DDOS ddos = Instantiate(self, new Vector2(transform.position.x - (1f * i), transform.position.y), Quaternion.identity).GetComponent<DDOS>();
            ddos.IsHead = false;
            ddos.Init(Path);
            ddos.CurrentIndex = CurrentIndex;
            ddos.CurrentTarget = CurrentTarget;
            GameManager.EnemySpawned();
            i++;
        }
        yield return null;
    }
}
