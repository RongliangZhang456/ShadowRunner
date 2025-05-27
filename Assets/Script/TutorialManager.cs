using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI hintText; // ��ק��ֵ
    public float slowMotionScale = 0.3f;

    public void StartTutorial(string type)
    {
        switch (type)
        {
            case "Jump":
                hintText.text = LocalizationManager.Get("Press the space bar to jump!");
                break;
            case "Color":
                hintText.text = LocalizationManager.Get("Press C to switch colours!");
                break;
            case "Gravity":
                hintText.text = LocalizationManager.Get("Press LeftShift to reverse gravity!");
                break;
        }

        Time.timeScale = slowMotionScale;
        hintText.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Time.timeScale < 1f)
        {
            if (Input.GetKeyDown(KeyCode.Space)) ExitTutorial();
            else if (Input.GetKeyDown(KeyCode.C)) ExitTutorial();
            else if (Input.GetKeyDown(KeyCode.LeftShift)) ExitTutorial();
        }
    }

    void ExitTutorial()
    {
        Time.timeScale = 1f;
        hintText.gameObject.SetActive(false);
    }
}