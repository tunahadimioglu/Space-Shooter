using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private Player _player;
    public bool isEnemyFast = false;
    [SerializeField]
    private Animator _playerDeathExplosionAnim;
    private AudioSource _explosionSound;
    [SerializeField]
    private GameObject _enemyLaserPrefab;   
    private AudioSource _laserSound;
    private bool _isDead = false;
    [SerializeField]
        
    
    // Start is called before the first frame update
    void Start()
    {   
        
        bool willShoot = Random.Range(0f, 10.0f) > 7.0f;
        _player = GameObject.Find("Player").transform.GetComponent<Player>();
        _playerDeathExplosionAnim = GetComponent<Animator>();
        _explosionSound = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
        _laserSound = GameObject.Find("LaserShot_Sound").GetComponent<AudioSource>();
        
        if(_playerDeathExplosionAnim == null) {
            Debug.LogError("Player Explosion Animation is NULL");
        }
        if(_player == null){
            Debug.LogError("The Player is NULL");
        }
        if(_explosionSound == null) {
            Debug.LogError("The Explosion Sound is NULL");
        }
        if(_laserSound == null) {
            Debug.LogError("The Laser Sound is NULL");
        }

        if(willShoot && !isEnemyFast && !_isDead) {
            StartCoroutine(FireLaser());
        }

        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isDead) {
            EnemyMovement();
        }    
    }
    void EnemyMovement(){
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5.4f){
            Destroy(this.gameObject);
        }
        
    }

    void OnTriggerEnter2D(Collider2D other){
        
        if(other.tag == "Laser") {
            _explosionSound.Play();
            Destroy(other.gameObject);
            
            
            if(_player != null && !isEnemyFast) {
                _player.AddScore(10);  
            }
            else if(_player != null && isEnemyFast) {
                _player.AddScore(30);
            }
            
            ExecuteDeath();
            
            
        }
        else if(other.tag == "Player") {
            _explosionSound.Play();
            if (_player != null) {
                _player.Damage();
            }
            ExecuteDeath(); 
        }
    }


    public void EnemySpeedup(float speedIncrease) {
        isEnemyFast = true;
        _speed += speedIncrease;
        GetComponent<SpriteRenderer>().color = Color.red;
        
    }

    void ExecuteDeath() {
        _isDead = true;
        _speed = 0; 
        _playerDeathExplosionAnim.SetTrigger("OnEnemyDeath");
    
    
        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = false;
        }

        Destroy(this.gameObject, 2.5f);
    } 

    IEnumerator FireLaser() {
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        if(this.gameObject != null) {
            if(!_isDead) {
                Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -1.025f, 0), Quaternion.identity);
                _laserSound.Play();
            }
            
        }
        
        
    }



}
