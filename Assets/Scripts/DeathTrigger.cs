using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("bruh");
        if(other.CompareTag("Enemy")){
            Debug.Log("bruhvol2");
            GameManager.Lose();
        }    
    }
}
