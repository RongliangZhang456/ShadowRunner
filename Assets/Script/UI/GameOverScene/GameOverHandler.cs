using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    public TMP_Text gameOverText, timeSurvivedText, timeSurvivedCountText, scoreText, scoreCountText, starsCollectedText, starsCollectedCountText;
    public Button continueButton, quitButton;
    void Start()
    {
        continueButton.onClick.AddListener(() =>
        {
            // go back to game scene
        });
        quitButton.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScreen");
        });
    }

    void OnEnable()
    {
        //Todo: update counts elements
        gameOverText.text = LocalizationManager.Get("game over");
        timeSurvivedText.text = LocalizationManager.Get("time survived");
        scoreText.text = LocalizationManager.Get("score");
        starsCollectedText.text = LocalizationManager.Get("stars collected");


    }
}
