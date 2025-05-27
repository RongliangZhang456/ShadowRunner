using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundHandler : PanelBase
{
    public Button playButton;

    public override string PanelID => "BackGround";

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel("LevelSelect");

        });
    }
}