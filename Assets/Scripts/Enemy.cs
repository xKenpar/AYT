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

    public float MaxSpeed;
    public float SlowDownSpeed;
    float _speed;

    public virtual void Init(LineRenderer path) {
        _path = path;
        _positions = new Vector3[_path.positionCount];
        _path.GetPositions(_positions);
        NextTarget();
    }

    void Start() {
        _speed = MaxSpeed;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public virtual void Update() {
        if(Vector2.Distance(transform.position, _currentTarget) < 0.1f){
            NextTarget();
        }
    }

    void FixedUpdate(){
        _rigidbody.MovePosition(Vector2.MoveTowards(transform.position, _currentTarget, Time.fixedDeltaTime * _speed));
    }

    void NextTarget(){
        if(_currentIndex != _path.positionCount) {
            _currentTarget = _positions[_currentIndex];
            _currentIndex++;
        }
    }

    public void SetSpeed(float speed) {
        _speed = speed;
    }

    public virtual void OnDeath() {
        Destroy(this.gameObject);
    }
}
