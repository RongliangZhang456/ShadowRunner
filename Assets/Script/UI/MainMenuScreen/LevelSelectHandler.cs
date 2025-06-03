using UnityEngine;
using UnityEngine.UI;

public class LevelSelectHandler : PanelBase
{
    public Button[] levelButtons;
    public Button backButton;
    public Text levelSelectText;
    public Text backButtonText;

    public override string PanelID => "LevelSelect";

    private void Start()
    {
        this.gameObject.SetActive(false);

        levelButtons[0].onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialScene");
            GameStatsManager.Instance.ResetStats();
            Time.timeScale = 1f;
        });
        //TODO: Replace with actual level names
        levelButtons[1].onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Color-change");
            GameStatsManager.Instance.ResetStats();
            Time.timeScale = 1f;
        });
        levelButtons[2].onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Anti-gravity");
        });


        backButton.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("LevelSelect");
        });
    }

    public void OnEnable()
    {
        levelSelectText.text = LocalizationManager.Get("level select");
        backButtonText.text = LocalizationManager.Get("back");
    }
}
