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

        levelButtons[0].onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialScene"));
        //TODO: Replace with actual level names
        // levelButtons[1].onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(""));
        // levelButtons[2].onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(""));


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
