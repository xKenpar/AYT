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

    public override void Init(LineRenderer path) {
        base.Init(path);
        StartCoroutine(Recovery());
        GameObject[] objs = GameObject.FindGameObjectsWithTag("BackDoor");
        if (objs.Length > 1) {
            _spawnNumber++;
            BackDoor backDoor = objs[0].GetComponentInChildren<BackDoor>();
            backDoor.GetComponentInChildren<BoxCollider2D>().enabled = true;
            Debug.Log("pog");
            backDoor.state = State.Walk;
            Destroy(transform.parent.gameObject);
        }
        _spawnNumber++;
        state = State.Walk;
    }

    public override void Update() {
        if(state == State.Walk){
            base.Update();
        }
    }

    public override void OnDeath() {
        //TODO(eren): idle animation
        
        _boxCollider2D.enabled = false;
        
        state = State.Idle;
        if(_spawnNumber > 0){
            Debug.Log("pog2");
            _recover = true;
        }
        _spawnNumber--;
        GameManager.EnemyDied();
    }

    IEnumerator Recovery() {
        while(_recover){
            yield return new WaitForSeconds(1f);
            _boxCollider2D.enabled = true;
            state = State.Walk;
            _spawnNumber--;
            _recover = false;
        }
    }
}
