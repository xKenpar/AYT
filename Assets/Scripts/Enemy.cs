using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] LineRenderer Path;
    Vector3[] _positions;
    
    Vector3 _currentTarget;
    int _currentIndex = 0;

    Rigidbody2D _rigidbody;
    [SerializeField] float Speed;

    void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();

        _positions = new Vector3[Path.positionCount];
        Path.GetPositions(_positions);
        foreach(Vector3 position in _positions)
            Debug.Log(position);
        NextTarget();
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
        if(_currentIndex != Path.positionCount) {
            _currentTarget = _positions[_currentIndex];
            _currentIndex++;
        }
    }

    public void SetSpeed(float speed) {
        Speed = speed;
    }
}
