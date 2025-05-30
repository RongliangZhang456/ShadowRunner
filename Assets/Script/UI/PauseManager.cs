using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : PanelBase
{
    public GameObject PausePanel;
    public Button ResumeButton, RestartButton, QuitButton, HomeButton;
    private bool isPaused = false;

    private void Start()
    {
        PausePanel.SetActive(false);

        ResumeButton.onClick.AddListener(ResumeGame);
        RestartButton.onClick.AddListener(RestartScene);
        QuitButton.onClick.AddListener(QuitGame);
        HomeButton.onClick.AddListener(GoToHome);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        PausePanel.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        PausePanel.SetActive(false);
    }

    private void RestartScene()
    {
        Time.timeScale = 1f;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameOverScene");
    }
    private void GoToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScreen");
    }
}