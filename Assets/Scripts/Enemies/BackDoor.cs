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
    int _counter = 0;
    bool _canRecover = false;
    float _recoveryTimer;

    BoxCollider2D _boxCollider2D;
    [SerializeField] float RecoveryTime;

    void Awake() {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _recoveryTimer = RecoveryTime;
    }
    public override void Init(LineRenderer path) {
        base.Init(path);
        
        GameObject[] objs = GameObject.FindGameObjectsWithTag("BackDoor");
        if (objs.Length > 1) {
            GameObject obj = objs[0];
            BackDoor backDoor = obj.GetComponentInChildren<BackDoor>();
            backDoor.Walk();
            backDoor._counter++;
            Destroy(transform.parent.gameObject);
        }
        _counter++;
        Walk();
    }

    void Walk(){
        state = State.Walk;
        _boxCollider2D.enabled = true;
    }

    void Idle(){
        state = State.Idle;
        _boxCollider2D.enabled = false;
        _counter--;
    }

    public override void Update() {
        if(state == State.Walk){
            base.Update();
        }
        else if(state == State.Idle && _canRecover){
            if(_recoveryTimer >= 0){
                _recoveryTimer -= Time.deltaTime;
            }
            else{
                _recoveryTimer = RecoveryTime;
                _canRecover = false;
                GameManager.EnemyDied();
                GetComponent<EnemyHealth>().Recover();
                animator.SetBool("died", false);
                Walk();
            }
        }
    }

    public override void FixedUpdate() {
        if(state == State.Walk){
            base.FixedUpdate();
        }
    }

    public override void OnDeath() {
        //TODO(eren): idle animation
        animator.SetBool("died", true);
        Idle();
        if(_counter > 0){
            _canRecover = true;
        } 
        else{
            GameManager.EnemyDied();
        }
    }
}
