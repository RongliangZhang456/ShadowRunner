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
<<<<<<< HEAD

=======
<<<<<<< HEAD
>>>>>>> 9a1ff07 (User Interface design)
=======
>>>>>>> 6220a74 (User Interface design)
>>>>>>> 2973359 (User Interface design)
    }
}