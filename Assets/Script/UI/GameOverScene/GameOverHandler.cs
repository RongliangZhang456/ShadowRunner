using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    public TMP_Text gameOverText, timeSurvivedText, timeSurvivedCountText, restartText, restartCountText, starsCollectedText, starsCollectedCountText;
    public Button continueButton, quitButton;
    void Start()
    {
        continueButton.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScreen");
            GameStatsManager.Instance.ResetStats();
            GameStatsManager.Instance.PauseTracking();
        });
        quitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
        });
    }

    void OnEnable()
    {
        //Todo: fix swedish error
        // timeSurvivedText.text = LocalizationManager.Get("time survived");
        // restartText.text = LocalizationManager.Get("restart count");
        // starsCollectedText.text = LocalizationManager.Get("stars collected");
        if (GameStatsManager.Instance != null)
        {
            timeSurvivedCountText.text = $"{GameStatsManager.Instance.playTime:F2} sec";
            starsCollectedCountText.text = $"{GameStatsManager.Instance.starsCollected}";
            restartCountText.text = $"{GameStatsManager.Instance.restartCount}";
        }


    }
}
