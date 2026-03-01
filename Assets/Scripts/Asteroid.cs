using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float rotationSpeed = 3.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private SpawnManager _spawnManager;
    private AudioSource _explosionSound;
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _explosionSound = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
        if(_spawnManager == null) {
            Debug.LogError("The Spawn Manager is NULL");
        }
        if(_explosionSound == null) {
            Debug.LogError("The Explosion Sound is NULL");
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        AsteroidMovement();
    }
    void AsteroidMovement() {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        
    }
    // void OnTriggerEnter2D(Collider2D other) {
    //     if(other.tag == "Laser") {
    //         Destroy(other.gameObject);
    //         GameObject newExplosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
    //         Animator anim = newExplosion.GetComponent<Animator>();
    //         if(anim != null) {
    //             anim.SetTrigger("LaserHit");
    //         }

    //         Destroy(this.gameObject, 2.7f);
    //     }
    // }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Laser") {
            _explosionSound.Play();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.2f);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
        }
    }

    
}
