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
            StartCoroutine(SpawnDDOSes(Random.Range(MinClones, MaxClones), CurrentIndex, CurrentTarget));
        }
    }

    IEnumerator SpawnDDOSes(int maxDDOS, int index, Vector2 target){
        while(i < maxDDOS){
            yield return new WaitForSeconds(.3f);
            DDOS ddos = Instantiate(self, new Vector2(transform.position.x - (0.4f * i), transform.position.y), Quaternion.identity).GetComponent<DDOS>();
            ddos.IsHead = false;
            ddos.Init(Path);
            ddos.CurrentIndex = index;
            ddos.CurrentTarget = target;
            GameManager.EnemySpawned();
            i++;
        }
        yield return null;
    }
}
