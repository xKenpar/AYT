using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public LineRenderer Path;
    Vector3[] _positions;
    
    Vector3 _currentTarget;
    public int CurrentIndex = 0;

    Rigidbody2D _rigidbody;

    public float MaxSpeed;
    public float SlowDownSpeed;
    float _speed;

    public virtual void Init(LineRenderer path) {
        Path = path;
        _positions = new Vector3[Path.positionCount];
        Path.GetPositions(_positions);
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
        if(CurrentIndex != Path.positionCount) {
            _currentTarget = _positions[CurrentIndex];
            CurrentIndex++;
        }
    }

    public void SetSpeed(float speed) {
        _speed = speed;
    }

    public virtual void OnDeath() {
        GameManager.EnemyDied();
        Destroy(this.gameObject);
    }
}
