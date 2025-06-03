using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TryAgainManager : PanelBase
{
    public GameObject TryAgainPanel, Player;
    public Button RestartButton, QuitButton;
    public Text RestartText, QuitText;

    void Start()
    {
        TryAgainPanel.SetActive(false);
        RestartButton.onClick.AddListener(RestartScene);
        QuitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (Player.GetComponent<PlayerController>().isGameOver)
        {
            Time.timeScale = 0f;
            TryAgainPanel.SetActive(true);
            Player.GetComponent<PlayerController>().isGameOver = false;
        }
    }

    private void RestartScene()
    {
        Time.timeScale = 1f;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        GameStatsManager.Instance.RestartStats();
    }

    private void QuitGame()
    {
        Time.timeScale = 1f;
        GameStatsManager.Instance.PauseTracking();
        SceneManager.LoadScene("GameOverScene");
    }
}
