using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class BackGroundHandler : PanelBase
{
    public Button playButton;
    public Button settingsButton;

    public override string PanelID => "BackGround";

    private void Start()
    {
        settingsButton.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel("LanguageSelect");
        });
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            UIManager.Instance.ShowPanel("LevelSelect");
        }
    }
}