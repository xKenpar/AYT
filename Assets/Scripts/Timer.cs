using System;
using UnityEngine;

public class Timer
{
    float _duration;
    bool _actionOver = false;
    Action _action;
    
    public Timer(float duration, Action action) {
        SetDuration(duration);
        OnTimeout(action);
    }

    public void SetDuration(float duration) {
        _duration = duration; _actionOver = false;
    }

    public void OnTimeout(Action action) {
        _action = action;
    }

    public void Update(float deltaTime) {
        if (_duration > 0){
            _duration -= deltaTime; 
        }
        else if (_duration <= 0 && !_actionOver){
            _action();
            _actionOver = true;
        }
    }
}
