using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackDoor : Enemy
{
    public enum State
    {
        Walk,
        Idle,
    }

    public State state;
    int _spawnNumber = 0;
    bool _recover = false;

    public override void Init(LineRenderer path) {
        base.Init(path);
        StartCoroutine(Recovery());
        GameObject[] objs = GameObject.FindGameObjectsWithTag("BackDoor");
        if (objs.Length > 1) {
            _spawnNumber++;
            BackDoor backDoor = objs[0].GetComponentInChildren<BackDoor>();
            backDoor.state = State.Walk;
            Destroy(this.gameObject);
        }

        state = State.Walk;
    }

    public override void Update() {
        if(state == State.Walk){
            base.Update();
        }
    }

    public override void OnDeath() {
        //TODO(eren): idle animation
        state = State.Idle;
        _spawnNumber--;
        if(_spawnNumber > 0){
            _recover = true;
        }
    }

    IEnumerator Recovery() {
        while(_recover){
            yield return new WaitForSeconds(1f);
            state = State.Walk;
            _spawnNumber--;
            _recover = false;
        }
    }
}
