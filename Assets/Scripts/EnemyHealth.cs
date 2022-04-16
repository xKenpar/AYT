using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Enemy _enemy;
    
    Timer _stunTimer, _slowDownTimer;
    float _posionTimer, _tempTimer = 0f;
    
    [SerializeField] float SlowDownSpeed = 1f;
    [SerializeField] float NormalSpeed = 2f;
    
    [SerializeField] float MaxHealth;
    [SerializeField] float PosionDamage;
    float _health;

    void Start() {
        _enemy = GetComponent<Enemy>();
        _health = MaxHealth;
        GetHit(20, 2f, 0f, 5f);
    }

    void Update() {
        if(_stunTimer != null){
            _stunTimer.Update(Time.deltaTime);
        }
        if(_slowDownTimer != null){
            _slowDownTimer.Update(Time.deltaTime);
        }
        if(_posionTimer >= 0f){
            if(_tempTimer <= 0){
                Damage(PosionDamage);
                _tempTimer = 1f;
            }
            _tempTimer -= Time.deltaTime;
            _posionTimer -= Time.deltaTime;
        }
    }

    public void GetHit(float damage, float poisonTime = 0f, float slowDownTime = 0f, float stunTime = 0f) {
        if(stunTime > 0f){
            _enemy.SetSpeed(0f);
            _stunTimer = new Timer(stunTime, () => {
                _enemy.SetSpeed(NormalSpeed);
            });
        }
        if(slowDownTime > 0f){
            _enemy.SetSpeed(SlowDownSpeed);
            _slowDownTimer = new Timer(slowDownTime, () => {
                _enemy.SetSpeed(NormalSpeed);
            });
        }
        _tempTimer = 1f;
        _posionTimer = poisonTime;
        
        Damage(damage);
    }

    void Damage(float damage) {
        _health -= damage;
        if(_health <= 0f){
            //öl
        }
    }
}
