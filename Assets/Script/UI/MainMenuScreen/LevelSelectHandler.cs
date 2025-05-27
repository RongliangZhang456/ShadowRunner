using UnityEngine;
using UnityEngine.UI;

public class LevelSelectHandler : PanelBase
{
    public Button[] levelButtons;
    public Button optionsButton;
    public Button backButton;
    public Text levelSelectText;
    public Text backButtonText;

    public override string PanelID => "LevelSelect";

    private void Start()
    {
        this.gameObject.SetActive(false);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i;
            levelButtons[i].onClick.AddListener(() => SelectLevel(index + 1));
        }
        optionsButton.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("LevelSelect");
            UIManager.Instance.ShowPanel("LanguageSelect");
        });

        backButton.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("LevelSelect");
            UIManager.Instance.ShowPanel("BackGround");
        });
    }

    void SelectLevel(int levelIndex)
    {
        Debug.Log("Level Selected: " + levelIndex);
    }
    public void OnEnable()
    {
        levelSelectText.text = LocalizationManager.Get("level select");
        backButtonText.text = LocalizationManager.Get("back");
    }
}
