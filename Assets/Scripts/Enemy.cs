using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    LineRenderer _path;
    Vector3[] _positions;
    
    Vector3 _currentTarget;
    int _currentIndex = 0;

    Rigidbody2D _rigidbody;
    [SerializeField] float Speed;

    public void Init(LineRenderer path) {
        _path = path;
        _positions = new Vector3[_path.positionCount];
        _path.GetPositions(_positions);
        NextTarget();
    }

    void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(Vector2.Distance(transform.position, _currentTarget) < 0.1f){
            NextTarget();
        }
    }

    void FixedUpdate(){
        _rigidbody.MovePosition(Vector2.MoveTowards(transform.position, _currentTarget, Time.fixedDeltaTime * Speed));
    }

    void NextTarget(){
        if(_currentIndex != _path.positionCount) {
            _currentTarget = _positions[_currentIndex];
            _currentIndex++;
        }
    }

    public void SetSpeed(float speed) {
        Speed = speed;
    }
}
