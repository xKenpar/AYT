using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public LineRenderer Path;
    Vector3[] _positions;
    
    public Vector3 CurrentTarget;
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
        Init(Path);
    }

    public virtual void Update() {
        if(Vector2.Distance(transform.position, CurrentTarget) < 0.1f){
            NextTarget();
        }
    }

    public virtual void FixedUpdate(){
        _rigidbody.MovePosition(Vector2.MoveTowards(transform.position, CurrentTarget, Time.fixedDeltaTime * _speed));
    }

    void NextTarget(){
        if(CurrentIndex != Path.positionCount) {
            CurrentTarget = _positions[CurrentIndex];
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
