using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private TMP_Text _restartText;
    [SerializeField]
    public TMP_Text _countdownText;
    private SpawnManager _spawnManager;
    private Player _player;
    [SerializeField]
    private TMP_Text tripleShotCountdownText;
    [SerializeField]
    private TMP_Text speedBoostCountdownText;
    [SerializeField]
    private TMP_Text shieldBoostCountdownText;
    [SerializeField]
    private GameObject[] _engines;      
    

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        
        if(_player == null) {
            Debug.LogError("The Player is NULL");
        }

        if(_spawnManager == null) {
            Debug.LogError("The Spawn Manager is NULL");
        }
        _scoreText.text = "Score : 0";
        _gameOverText.gameObject.SetActive(false);
        tripleShotCountdownText.gameObject.SetActive(false);
        speedBoostCountdownText.gameObject.SetActive(false);
        shieldBoostCountdownText.gameObject.SetActive(false);
    
        StartCoroutine(CountDownRoutine());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateScore(int PlayerScore) {
        _scoreText.text = "Score : " + PlayerScore.ToString();
    }
    public void UpdateLives(int currentLives) {
        _livesImage.sprite = _livesSprites[currentLives];
    }
    public void DisplayGameOver() {
        StartCoroutine(GameOverFlickerRoutine());

    } 
    IEnumerator GameOverFlickerRoutine() {
        while(true) {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void DisplayRestartText() {
        _restartText.gameObject.SetActive(true);
    }



    IEnumerator CountDownRoutine() {
        int count = 3;
        while(count > 0) {
            _countdownText.text = count.ToString();
            yield return new WaitForSeconds(1.0f);
            count--;
        }
        _countdownText.text = "GO!";
        yield return new WaitForSeconds(1.0f);
        _countdownText.gameObject.SetActive(false);
        if(_spawnManager != null && _player != null) {
            _player.canMove = true;
            
        }
    }
    


    

    public void UpdateTripleShotCountdown(float timeLeft) {
         
        tripleShotCountdownText.gameObject.SetActive(true);
        tripleShotCountdownText.text = "Triple Shot: " + timeLeft.ToString("F1") + "s";

        if(timeLeft <= 0.01f) {
            tripleShotCountdownText.gameObject.SetActive(false);
        }
            
    
    }

    public void UpdateSpeedBoostCountdown(float timeLeft) {
        speedBoostCountdownText.gameObject.SetActive(true);
        speedBoostCountdownText.text = "Speed Boost: " + timeLeft.ToString("F1") + "s";
        if(timeLeft <= 0.01f) {
            speedBoostCountdownText.gameObject.SetActive(false);
        }
    }

    public void UpdateShieldBoostCountdown(float timeLeft) {
        shieldBoostCountdownText.gameObject.SetActive(true);
        shieldBoostCountdownText.text = "Shield: " + timeLeft.ToString("F1") + "s";
        if(timeLeft <= 0.01f) {
            shieldBoostCountdownText.gameObject.SetActive(false);
        }
        
    }
    public void HideShieldCountdown() {
        shieldBoostCountdownText.gameObject.SetActive(false);
    }

    public void ActivateRandomEngine() {
        bool isLeftEngine = Random.Range(0f,10f) > 5f;
        if(isLeftEngine) {
            _engines[0].SetActive(true);
        }
        else {
            _engines[1].SetActive(true);
        }
        
    }

    public void ActivateRightEngine() {
        _engines[1].SetActive(true);
    }

    public void ActivateLeftEngine() {
        _engines[0].SetActive(true);
    }

    public bool isLeftEngineActive() {
        return _engines[0].activeSelf;
    }

    
    
    
    
}
