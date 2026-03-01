using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _pauseMenu;
    public bool isPaused = false;
    private UIManager _uiManager;


    void Start()
    {
        _pauseMenu.SetActive(false);
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null) {
            Debug.LogError("The UI Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPaused && Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }
        else if(isPaused && Input.GetKeyDown(KeyCode.Escape)) { 
            ResumeGame();
        }
    }
    public void PauseGame () {
        if (_uiManager._countdownText.gameObject.activeSelf) {
            _uiManager._countdownText.enabled = false; 
        }
        
        isPaused = true;
        _pauseMenu.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ResumeGame () {
        
        isPaused = false;
        _pauseMenu.SetActive(false);
        _uiManager._countdownText.enabled = true;
        Time.timeScale = 1f;

        if(EventSystem.current != null) {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void ReturnToMainMenu() {
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // Main menu scene.
    }

    public void Quit() {
        Application.Quit();
    }
}
