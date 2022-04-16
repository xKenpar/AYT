using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterShooter : MonoBehaviour
{
    [SerializeField] float shotDelay = 1f;

    [SerializeField] float speed = 5;
    [SerializeField] float poisonTime = 0, slowDownTime = 0, stunTime = 0, splashRadius = 0, damage = 5;

    [SerializeField] bool boomerang = false;

    [SerializeField] bool permanent = false;

    [SerializeField] bool piercing = false;


    [SerializeField] GameObject bulletPrefab;
    List<Transform> _targets = new List<Transform>();

    float _timer = 0f;
    Letter _letter;
    BulletData data;
    void Start() {
        _letter = GetComponentInParent<Letter>();
        data = new BulletData(speed, poisonTime, slowDownTime, stunTime, splashRadius, damage, boomerang, permanent, piercing);
    }
    void Update() {
        if (_targets.Count > 0 && GetComponentInParent<Letter>().assignedSlot != null && !_letter.dragging) {
            if (_timer > shotDelay) {
                _timer = 0;
                Shoot();
            } else {
                _timer += Time.deltaTime;
            }
        } else {
            if(_timer <= shotDelay)
                _timer += Time.deltaTime;
        }
    }
    void Shoot() {
        while(_targets.Count > 0 && _targets[0] == null) {
            _targets.RemoveAt(0);
        }

        if (_targets.Count == 0) return;

        Instantiate(bulletPrefab, transform.position,  Quaternion.identity).GetComponent<Bullet>().Init(_targets[0], data);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            _targets.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            _targets.Remove(collision.transform);
        }
    }
}
