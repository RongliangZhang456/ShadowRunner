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

<<<<<<< HEAD
        levelButtons[0].onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialScene"));
        //TODO: Replace with actual level names
        levelButtons[1].onClick.AddListener(() =>
        UnityEngine.SceneManagement.SceneManager.LoadScene("Anti-gravity"));
        levelButtons[2].onClick.AddListener(() =>
            UnityEngine.SceneManagement.SceneManager.LoadScene("Color-change"));


=======
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i;
            levelButtons[i].onClick.AddListener(() => SelectLevel(index + 1));
        }
>>>>>>> 9a1ff07 (User Interface design)
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

<<<<<<< HEAD
=======
    void SelectLevel(int levelIndex)
    {
        Debug.Log("Level Selected: " + levelIndex);
    }
>>>>>>> 9a1ff07 (User Interface design)
    public void OnEnable()
    {
        levelSelectText.text = LocalizationManager.Get("level select");
        backButtonText.text = LocalizationManager.Get("back");
    }
}
