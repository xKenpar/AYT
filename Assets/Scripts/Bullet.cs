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

    public void Init(float speed, Transform targetTransform, float poisonTime = 0, float slowDownTime = 0, float stunTime = 0 , float splashRadius = 0, float damage = 5,
                     bool boomerang = false, bool permanent = false, bool piercing = false) {
        _startPosition = transform.position;
        _speed = speed;
        
        _targetTransform = targetTransform;
        _lastTargetDirection = (targetTransform.position-transform.position).normalized;

        _poisonTime = poisonTime; 
        _slowDownTime = slowDownTime;
        _stunTime = stunTime;
        _splashRadius = splashRadius;
        _canReturn = boomerang;
        _permanent = permanent;
        _piercing = piercing;
        if(_piercing)
            TrailParticle.SetActive(true);

        _damage = damage;
        if(_damage > 10f)
            transform.localScale = new Vector3(.4f, .4f, .4f);
        
    }

    void FixedUpdate() {
        if(_waiting)
            return;

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
