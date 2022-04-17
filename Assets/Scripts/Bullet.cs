using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] LayerMask EnemyLayerMask;

    [SerializeField] GameObject SmallSplashParticle, BigSplashParticle;

    [SerializeField] GameObject TrailParticle;

    Rigidbody2D _rigidbody2D;
    Vector3 _startPosition; 
    Transform _targetTransform;
    BoxCollider2D _targetBoxCollider2D;
    Vector3 _lastTargetDirection;

    float _speed; 
    float _poisonTime, _slowDownTime, _stunTime, _splashRadius,_damage;

    bool _canReturn;
    bool _isReturning = false;
    
    bool _permanent;
    bool _waiting = false;

    bool _piercing;

    void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Init(Transform targetTransform, BulletData data) {
        _startPosition = transform.position;
        _speed = data._speed;
        
        _targetTransform = targetTransform;
        _targetBoxCollider2D = _targetTransform.GetComponent<BoxCollider2D>();
        _lastTargetDirection = (targetTransform.position-transform.position).normalized;

        _poisonTime = data._poisonTime; 
        _slowDownTime = data._slowDownTime;
        _stunTime = data._stunTime;
        _splashRadius = data._splashRadius;
        _canReturn = data._boomerang;
        _permanent = data._permanent;
        _piercing = data._piercing;
        if(_piercing)
            TrailParticle.SetActive(true);

        _damage = data._damage;
        if(_damage > 10f)
            transform.localScale = new Vector3(.4f, .4f, .4f);
        
    }

    void FixedUpdate() {
        if(_waiting)
            return;

        if(_targetBoxCollider2D && !_targetBoxCollider2D.enabled)
            _targetTransform = null;

        if(_targetTransform)
           _lastTargetDirection = _targetTransform.position-transform.position;

        if(_isReturning)
            _lastTargetDirection = _startPosition-transform.position;
        
        _rigidbody2D.MovePosition(transform.position + _lastTargetDirection.normalized * _speed * Time.fixedDeltaTime);

        if(_isReturning && Vector2.Distance(_startPosition,transform.position) < .1f)
            GetDestroyed();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")){
            //Hit Enemies
            other.GetComponent<EnemyHealth>().GetHit(_damage,_poisonTime,_slowDownTime,_stunTime);
            if(_splashRadius > 0f){
                if(_splashRadius > 1f)
                    Instantiate(BigSplashParticle, transform.position, Quaternion.identity);
                else
                    Instantiate(SmallSplashParticle, transform.position, Quaternion.identity);
                    
                foreach(Collider2D enemy in Physics2D.OverlapCircleAll(transform.position, _splashRadius, EnemyLayerMask)){
                    enemy.GetComponent<EnemyHealth>().GetHit(_damage,_poisonTime,_slowDownTime,_stunTime);
                }
            }
            //Check if pierced
            if(_piercing && other.transform != _targetTransform && !_waiting)
                return;
            
            //stop in place if permanent
            if(_permanent){
                _permanent = false;
                _waiting = true;
                return;
            } else if(_waiting) //stop waiting 
                _waiting = false;

            //Check if boomerang should return back
            if(_canReturn){
                _canReturn = false;
                _isReturning = true;
                return;
            }

            //Destroy Bullet
            GetDestroyed();
        }    
    }

    void GetDestroyed(){
        Destroy(gameObject);
    }

}

public class BulletData {
    public BulletData(float speed = 5, float poisonTime = 0, float slowDownTime = 0, float stunTime = 0,
        float splashRadius = 0, float damage = 5, bool boomerang = false, bool permanent = false, bool piercing = false, float shotDelay = 1, float zoneSize = 2.5f) {
        _speed = speed;
        _poisonTime = poisonTime;
        _slowDownTime = slowDownTime;
        _stunTime = stunTime;
        _splashRadius = splashRadius;
        _damage = damage;
        _boomerang = boomerang;
        _permanent = permanent;
        _piercing = piercing;
        _shotDelay = shotDelay;
        _zoneSize = zoneSize;
    }

    public float _speed = 5;
    public float _poisonTime = 0, _slowDownTime = 0, _stunTime = 0, _splashRadius = 0, _damage = 5, _shotDelay = 1, _zoneSize = 2.5f;

    public bool _boomerang = false;

    public bool _permanent = false;

    public bool _piercing = false;

    public BulletData MergeBulletData(BulletData data1, BulletData data2) {
        return new BulletData(Mathf.Max(data1._speed, data2._speed), Mathf.Max(data1._poisonTime, data2._poisonTime),
            Mathf.Max(data1._slowDownTime, data2._slowDownTime), Mathf.Max(data1._stunTime, data2._stunTime),
            Mathf.Max(data1._splashRadius, data2._splashRadius), Mathf.Max(data1._damage, data2._damage),
            data1._boomerang || data2._boomerang, data1._permanent || data2._permanent, data1._piercing || data2._piercing
            );
    }
}




