using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageSelectHandler : PanelBase
{
    public TMP_Dropdown dropDown;
    public Button backButton;
    public Text optionsText;
    public Text languageText;
    public Text dropDownText;
    public Text backButtonText;
    public Text playButtonText;

    public override string PanelID => "LanguageSelect";

    private void Start()
    {
        this.gameObject.SetActive(false);
        dropDown.onValueChanged.AddListener(index =>
        {
            dropDownText.text = dropDown.options[index].text;

            LocalizationManager.LoadLanguage(dropDown.options[index].text);
            optionsText.text = LocalizationManager.Get("options");
            languageText.text = LocalizationManager.Get("language");
            backButtonText.text = LocalizationManager.Get("back");
            playButtonText.text = LocalizationManager.Get("press enter button");
        });

        backButton.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("LanguageSelect");
            UIManager.Instance.ShowPanel("BackGround");
        });
    }
}