using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    Enemy _enemy;

    void Start() {
        _enemy = GetComponent<Enemy>();
    }

    void Update() {
        
    }

    public void GetHit(float damage, float poison = 0, float slowDown = 0, float stunTime = 0) {
        
    }
}
