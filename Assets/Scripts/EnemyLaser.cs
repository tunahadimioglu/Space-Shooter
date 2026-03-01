using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    private float _speed = 9.0f;
    private Player _player;
    
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        
        if(_player == null){
            Debug.LogError("The Player is NULL");
        }
    }

    
    void Update()
    {
        EnemyLaserMovement();
    }

    void EnemyLaserMovement() {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
        if(transform.position.y < -5.4f) {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            if(_player != null) {
                _player.Damage();
            }
            Destroy(this.gameObject);
        }
        else if(other.tag == "Laser") {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
