using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    private WaitForSeconds _enemySpawnWait = new WaitForSeconds(2.0f);
    [SerializeField]
    private GameObject _enemyPrefab; // -5.4f
    [SerializeField]
    private GameObject _enemyContainer;
    private bool stopSpawning = false;
    [SerializeField]
    private GameObject _TripleShotPowerUp;
    [SerializeField]
    private GameObject _powerUpContainer;
    [SerializeField]
    private GameObject _speedPowerUp;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private GameObject _asteroid;
    

      

    

    void Start()
    {   
        Instantiate(_asteroid, new Vector3(0,5,0), Quaternion.identity);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartSpawning() {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine(){
        yield return new WaitForSeconds(2.0f);
        while(stopSpawning == false) {
            
            bool isFast = Random.Range(0f,10f) > 8;
            float xRange = isFast ? 7.2f : 9.2f;
            float randomX = Random.Range(-xRange, xRange);
            Vector3 spawnPosition = new Vector3(randomX, 7.4f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            if(isFast) {
                Enemy enemyScript = newEnemy.GetComponent<Enemy>();
                if(enemyScript != null) {
                    enemyScript.EnemySpeedup(2.5f); // Increase by 2.5
                }
            }
            
            yield return _enemySpawnWait;
        }
    }
    
    IEnumerator SpawnPowerupRoutine(){
        while(!stopSpawning) {
            yield return new WaitForSeconds(Random.Range(8f, 11f));
            float randomX = Random.Range(-9.2f, 9.2f);
            int _powerupIndex = Random.Range(0,3);
            Vector3 spawnPosition = new Vector3(randomX, 7.4f, 0);
            if(stopSpawning == false) {
                GameObject newPowerup = Instantiate(powerups[_powerupIndex], spawnPosition, Quaternion.identity);
                newPowerup.transform.parent = _powerUpContainer.transform;
            }
            
        
        }
    }

    // IEnumerator SpawnPowerupSpeedRoutine() {
    //     while(stopSpawning == false) {
    //         yield return new WaitForSeconds(Random.Range(15f, 20f));
    //         float randomX = Random.Range(-9.2f, 9.2f);
    //         Vector3 spawnPosition = new Vector3(randomX, 7.4f, 0);
    //         GameObject newSpeedPowerup = Instantiate(_speedPowerUp, spawnPosition, Quaternion.identity);
    //         newSpeedPowerup.transform.parent = _powerUpContainer.transform;
    //     }
    // }

    public void OnPlayerDeath() {
        stopSpawning = true;
        KillExisting();
    }

    void KillExisting() {
        foreach(Transform enemy in _enemyContainer.transform) {
            Destroy(enemy.gameObject);
        }
        foreach(Transform powerup in _powerUpContainer.transform) {
            Destroy(powerup.gameObject);
        }
    }

}



