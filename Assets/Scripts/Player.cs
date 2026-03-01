using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    [SerializeField] // to make private variable visible in unity
    private float _speed = 8.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float fireRateSingle = 0.3f;
    private float nextFire = 0.0f;
    private WaitForSeconds _tripleShotWait = new WaitForSeconds(0.2f);
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private bool _isTripleShotActive = false;
    private WaitForSeconds _tripleShotDuration = new WaitForSeconds(5.0f);
    private Coroutine _powerDownCoroutine;
    private Coroutine _speedDownCoroutine;
    private Coroutine _shieldDownCoroutine;
    [SerializeField]
    private bool isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    private GameManager _gameManager;
    public bool canMove = false;
    [SerializeField]
    private float _tripleShotPowerupTime = 5.0f; 
    private float _shieldPowerupTime = 5.0f;
    private float _speedPowerupTime = 5.0f;
    private PauseMenu _pauseMenu;
    private AudioSource _laserMusic;
    [SerializeField]
    private GameObject _explosionPrefab;    
    
    // Start is called before the first frame update
    void Start()
    {
        // take the current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _pauseMenu = GameObject.Find("Game_Manager").GetComponent<PauseMenu>();
        _laserMusic = GameObject.Find("LaserShot_Sound").GetComponent<AudioSource>();


        if(_spawnManager == null){
            Debug.LogError("The Spawn Manager is NULL");
        }
        if(_uiManager == null){
            Debug.LogError("The UI Manager is NULL");
        }
        if(_gameManager == null){
            Debug.LogError("The Game Manager is NULL");
        }
        if(_pauseMenu == null) {
            Debug.LogError("PauseMenu component is missing from GameManager.");
        }
        if(_laserMusic == null) {
            Debug.LogError("LaserShot_Music AudioSource is NULL");
        }



    }

    // Update is called once per frame
    void Update()
    {    
        if(canMove) {
            PlayerMovement();
        }
        

        if(Input.GetKeyDown(KeyCode.Space) && (Time.time > nextFire) && canMove && !_pauseMenu.isPaused){
            FireLaser();
        }


    
    }

    void PlayerMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3
        (
            transform.position.x,
            Mathf.Clamp(transform.position.y, -3.9f, 0),
            0
        );
        if(transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if(transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);

        }

    }

    void FireLaser(){
        _laserMusic.Play();
        if(_isTripleShotActive) {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.025f, 0), Quaternion.identity);
        }
        nextFire = Time.time + fireRateSingle;
        
    }
    
    public void Damage(){
        if(isShieldActive) {
            isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            _uiManager.HideShieldCountdown();
            return;
        }
        else if(!isShieldActive) {
            _lives--;
            if(_lives == 2) {
                _uiManager.ActivateRandomEngine();
            }
            
            else if(_lives == 1 && _uiManager.isLeftEngineActive()) {
                _uiManager.ActivateRightEngine();
            }

            else if(_lives == 1 && !_uiManager.isLeftEngineActive()) {
                _uiManager.ActivateLeftEngine();
            }   
            _uiManager.UpdateLives(_lives);
        }
        
        if(_lives < 1){
            canMove = false;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.2f);
            _spawnManager.OnPlayerDeath();
            
            _uiManager.DisplayGameOver();
            _uiManager.DisplayRestartText();
            _gameManager.GameOver();
            
        }
    }
    
    // public void ActivateTripleShot() {
    //     _isTripleShotActive = true;

    //     if (_powerDownCoroutine != null) {
    //     StopCoroutine(_powerDownCoroutine);
    //     }

    //     _powerDownCoroutine = StartCoroutine(TripleShotPowerDownRoutine());
    // }

    // IEnumerator TripleShotPowerDownRoutine() {
    //     yield return _tripleShotDuration;
    //     _isTripleShotActive = false;
    // }

    public IEnumerator TripleShotRoutine() {
        _isTripleShotActive = true;
        float remainingTime = _tripleShotPowerupTime;
        while(remainingTime > 0) {
            _uiManager.UpdateTripleShotCountdown(remainingTime);
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        _isTripleShotActive = false;

    }
    
    // public void ActivateSpeedBoost() {
        
    //     _speed *= 1.5f;
    //     if(_speedDownCoroutine != null){
    //         StopCoroutine(_speedDownCoroutine);
    //     }
    //     _speedDownCoroutine = StartCoroutine(SpeedBoostDownRoutine());
    // }
    // IEnumerator SpeedBoostDownRoutine() {
    //     yield return new WaitForSeconds(3.0f);
    //     _speed /= 1.5f;
        
    // }

    public IEnumerator SpeedBoostRoutine() {
        _speed *= 1.5f;
        float remainingTime = _speedPowerupTime;
        while(remainingTime > 0) {
            _uiManager.UpdateSpeedBoostCountdown(remainingTime);
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        _speed /= 1.5f;
    }

    // public void ActivateShield() {
    //     isShieldActive = true;
    //     _shieldVisualizer.SetActive(true);
    //     if(_shieldDownCoroutine != null) {
    //         StopCoroutine(_shieldDownCoroutine);
    //     }
    //     _shieldDownCoroutine = StartCoroutine(ShieldPowerDownRoutine());

    // }
    // IEnumerator ShieldPowerDownRoutine() {
    //     yield return new WaitForSeconds(5.0f);
    //     _shieldVisualizer.SetActive(false);
    //     isShieldActive = false;
    // }

    public IEnumerator ShieldRoutine() {
        isShieldActive = true;
        _shieldVisualizer.SetActive(true);
        float remainingTime = _shieldPowerupTime;
        while(remainingTime > 0) {
            if(!isShieldActive) {
                yield break;
            }
            _uiManager.UpdateShieldBoostCountdown(remainingTime);
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        isShieldActive = false;
        _shieldVisualizer.SetActive(false);
    }

    public void AddScore(int points) {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    
} 
 