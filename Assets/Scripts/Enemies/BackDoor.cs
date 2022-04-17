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

    [SerializeField] BoxCollider2D _boxCollider2D;

    void Awake() {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }
//1 2 1
    public override void Init(LineRenderer path) {
        base.Init(path);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("BackDoor");
        if (objs.Length > 1) {
            BackDoor backDoor = objs[0].GetComponentInChildren<BackDoor>();
            backDoor.GetComponent<BoxCollider2D>().enabled = true;
            backDoor._spawnNumber++;
            backDoor.state = State.Walk;
            Destroy(transform.parent.gameObject);
        }
        StartCoroutine(Recovery());
        _spawnNumber++;
        state = State.Walk;
    }

    public override void Update() {
        if(state == State.Walk){
            base.Update();
        }
    }

    public override void FixedUpdate() {
        if(state == State.Walk){
            base.FixedUpdate();
        }
    }

    public override void OnDeath() {
        //TODO(eren): idle animation
        
        _boxCollider2D.enabled = false;
        
        state = State.Idle;
        _spawnNumber--;
        if(_spawnNumber > 0){
            _recover = true;
        }
        GameManager.EnemyDied();
    }

    IEnumerator Recovery() {
        while(true){
            if(_recover){
                Debug.Log("pog2");
                yield return new WaitForSeconds(1f);
                _boxCollider2D.enabled = true;
                state = State.Walk;
                _spawnNumber--;
                _recover = false;
            }
            yield return null;
        }
    }
}
