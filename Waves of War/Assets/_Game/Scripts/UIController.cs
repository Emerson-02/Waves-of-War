using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject GameScreen;
    public GameObject pausePanel;
    public GameObject finalScreen;
    public GameObject gameOverPanel;
    public GameObject optionsPanel;
    public GameObject menuPanel; 
    public GameObject tutorialPanel;
    public Slider spawnSlider, sessionTimeSlider; 
    public static int spawnValue, sessionTimeValue;
    public TextMeshProUGUI scoreText;
    public GameController gameController;
    public TMP_Text gameSessionTimeText;
    public TMP_Text spawnEnemyTimeText;
    public static bool tutorialShown = false;

    private void Awake() {
        gameController = FindObjectOfType<GameController>();

        if (spawnValue < 1)
        {
            spawnValue = 1;
        }

        if (sessionTimeValue < 1)
        {
            sessionTimeValue = 1;
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            spawnSlider.onValueChanged.AddListener(OnSpawnSliderValueChanged);
            sessionTimeSlider.onValueChanged.AddListener(OnSessionTimeSliderValueChanged);
        }

        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameScreen.SetActive(true);
            pausePanel.SetActive(false);
            finalScreen.SetActive(false);
            gameOverPanel.SetActive(false);

            if (!tutorialShown)
            {
                ShowTutorial();
                tutorialShown = true;
            }
        }
        
    }

    void Update()
    {

        if (SceneManager.GetActiveScene().name == "Game")
        {
            
            if (Time.timeScale == 0)
            {
                
                if (gameController.timeRemaining > 0 && PlayerController.instance.life > 0)
                {
                    
                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        
                        Time.timeScale = 1;
                        
                        pausePanel.SetActive(false);
                        
                        GameScreen.SetActive(true);
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    PauseGame();
                }                
            }

            


            
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            
            if (Input.GetKeyDown(KeyCode.M))
            {
                SceneManager.LoadScene("MainMenu");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (tutorialPanel.activeSelf)
                {
                    CloseTutorial();
                }
            }
        }
        
    }

    public void PauseGame()
    {
        Time.timeScale = 0; 
        pausePanel.SetActive(true); 
    }

    public void RestartGame()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene("Game"); 
        Time.timeScale = 1; 
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene("MainMenu");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; 
        pausePanel.SetActive(false);
    }

    public void ActivateFinalScreen()
    {
        finalScreen.SetActive(true);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
    }

    public void OpenOptions()
    {
        menuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void BackMenu()
    {
        optionsPanel.SetActive(false);
        menuPanel.SetActive(true); 
    }

    void OnSpawnSliderValueChanged(float value)
    {
        spawnValue = Mathf.RoundToInt(value);
        spawnEnemyTimeText.text = spawnValue.ToString();
    }

    void OnSessionTimeSliderValueChanged(float value)
    {
        sessionTimeValue = Mathf.RoundToInt(value);
        gameSessionTimeText.text = sessionTimeValue.ToString();
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            spawnSlider.value = spawnValue;
            sessionTimeSlider.value = sessionTimeValue;

            spawnEnemyTimeText.text = spawnValue.ToString();
            
            gameSessionTimeText.text = sessionTimeValue.ToString();

            optionsPanel.SetActive(false);
            menuPanel.SetActive(true);
        }
    }

    void ShowTutorial()
    {
        Time.timeScale = 0; 
        GameScreen.SetActive(false); 
        tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        Time.timeScale = 1; 
        GameScreen.SetActive(true); 
        tutorialPanel.SetActive(false); 
    }

}
