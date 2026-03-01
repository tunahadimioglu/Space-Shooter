using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 3.0f;
    private Player _player;
    private AudioSource _powerupSound;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _powerupSound = GameObject.Find("Powerup_Sound").GetComponent<AudioSource>();
        

        if(_player == null){
            Debug.LogError("The Spawn Manager is NULL");
        }
        if(_powerupSound == null) {
            Debug.LogError("The Powerup Sound is NULL");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        PowerUpMovement();
    }

    
    void PowerUpMovement() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5.4f){
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && this.gameObject.tag == "TripleShotPowerup") {
            _powerupSound.Play();
            Destroy(this.gameObject);
            _player.StartCoroutine(_player.TripleShotRoutine());
        }
        if(other.tag == "Player" && this.gameObject.tag == "SpeedPowerup") {
            _powerupSound.Play();
            Destroy(this.gameObject);
            _player.StartCoroutine(_player.SpeedBoostRoutine());
        }
        if(other.tag == "Player" && this.gameObject.tag == "ShieldPowerup") {
            _powerupSound.Play();
            Destroy(this.gameObject);
            _player.StartCoroutine(_player.ShieldRoutine());
        }
    }
}
