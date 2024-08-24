using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float timeRemaining;
    public int initialTimeInMinutes = 1;
    public bool timerIsRunning = false;
    public int score = 0;
    public UIController uiController;

    private void Awake() {
        uiController = FindObjectOfType<UIController>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        score = 0;

        initialTimeInMinutes = UIController.sessionTimeValue;

        if (initialTimeInMinutes < 1)
        {
            initialTimeInMinutes = 1;
        }

        timeRemaining = initialTimeInMinutes * 60;
        timerIsRunning = true;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                Time.timeScale = 0; 

                uiController.GameScreen.SetActive(false); 

                uiController.finalScreen.SetActive(true); 
                uiController.scoreText.text = "Final Score: " + score;

            }
        }

        if (PlayerController.instance.life <= 0)
        {
            timerIsRunning = false;
            Time.timeScale = 0; 

            uiController.GameScreen.SetActive(false); 

            uiController.gameOverPanel.SetActive(true);
        }


    }

    private void OnGUI()
    {
        // Resolução base
        float baseWidth = 1920f;
        float baseHeight = 1080f;

        // Resolução atual
        float currentWidth = Screen.width;
        float currentHeight = Screen.height;

        // Calcular a proporção
        float widthRatio = currentWidth / baseWidth;
        float heightRatio = currentHeight / baseHeight;

        // Usar a menor proporção para manter a consistência
        float ratio = Mathf.Min(widthRatio, heightRatio);

        // Calcular o tamanho da fonte ajustado
        int adjustedFontSize = Mathf.FloorToInt(50 * ratio);

        // Calcular o tempo restante
        int minutes = Mathf.FloorToInt(timeRemaining / 60F);
        int seconds = Mathf.FloorToInt(timeRemaining - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Criar e ajustar o estilo
        GUIStyle style = new GUIStyle();
        style.fontSize = adjustedFontSize;
        
        // Desenhar o rótulo
        GUI.Label(new Rect(10, 10, 250, 100), niceTime, style);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            Time.timeScale = 1;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
