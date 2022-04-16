using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleMan : Enemy
{
    enum State
    {
        Walk,
        Hide,
    }

    float _walkTimer = 3f, _hideTimer = 1f;
    State _state = State.Walk;

    [SerializeField] float HideSpeed;

    public override void Update() {
        base.Update();
        switch (_state)
        {
            case State.Walk: {
                if(_walkTimer >= 0){
                    _walkTimer -= Time.deltaTime;
                }
                else{
                    Hide();
                }
            };
            break;
            case State.Hide: {
                if(_hideTimer >= 0f){
                    _hideTimer -= Time.deltaTime;
                }
                else{
                    Walk();
                }
            };
            break;
        }
    }

    void Hide() {
        _state = State.Hide;
        _walkTimer = 3f;

        SetSpeed(HideSpeed);

        //TODO(eren): animasyonlar
    }

    void Walk() {
        _state = State.Walk;
        _hideTimer = 1f;

        SetSpeed(MaxSpeed);

        //TODO(eren): animasyonlar
    }
}
