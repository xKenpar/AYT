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

    public override void Init(LineRenderer path) {
        base.Init(path);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("BackDoor");
        if (objs.Length > 1) {
            BackDoor backDoor = objs[0].GetComponent<BackDoor>();
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
    }
}
