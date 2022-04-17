using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Enemy _enemy;
    
    Timer _stunTimer, _slowDownTimer;
    float _poisonTimer, _tempTimer = 0f;
    
    [SerializeField] float MaxHealth;
    [SerializeField] float PoisonDamage;
    float _health;

    void Start() {
        _enemy = GetComponent<Enemy>();
        _health = MaxHealth;
    }

    void Update() {
        if(_stunTimer != null){
            _stunTimer.Update(Time.deltaTime);
        }
        if(_slowDownTimer != null){
            _slowDownTimer.Update(Time.deltaTime);
        }
        if(_poisonTimer > 0f){
            if(_tempTimer <= 0){
                Damage(PoisonDamage);
                _tempTimer = 1f;
            }
            _tempTimer -= Time.deltaTime;
            _poisonTimer -= Time.deltaTime;
        }
    }
    
    public void GetHit(float damage, float poisonTime = 0f, float slowDownTime = 0f, float stunTime = 0f) {
        if(stunTime > 0f){
            _enemy.OnStun();
            _stunTimer = new Timer(stunTime, _enemy.OnStunOver);
        }
        if(slowDownTime > 0f){
            _enemy.SetSpeed(_enemy.SlowDownSpeed);
            _slowDownTimer = new Timer(slowDownTime, () => {
                _enemy.SetSpeed(_enemy.MaxSpeed);
            });
        }
        _tempTimer = 1f;
        
        if(poisonTime > 0f)
            _poisonTimer = poisonTime;
        
        Damage(damage);
    }

    public void Recover() {
        _health = MaxHealth;
    }
    
    void Damage(float damage) {
        _health -= damage;
        if(_health <= 0f){
            _enemy.OnDeath();
        }
    }
}
