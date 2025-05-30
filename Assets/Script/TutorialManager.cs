using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour, PlayerControls.IGameplayActions
{
    public TextMeshProUGUI hintText;
    public float slowMotionScale = 0.3f;

    private PlayerControls controls;
    private string currentTutorial = "";

    private InputDevice lastUsedDevice = null; // 记录最后使用的设备

    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.SetCallbacks(this);
        controls.Gameplay.Enable();

        hintText.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        controls.Gameplay.Disable();
    }

    // 开始教程，启动慢动作，显示提示
    public void StartTutorial(string type)
    {
        currentTutorial = type;
        UpdateHintText();

        Time.timeScale = slowMotionScale;
        hintText.gameObject.SetActive(true);
    }

    // 更新提示文本，支持多语言和手柄/键盘切换
    void UpdateHintText()
    {
        bool isGamepad = IsCurrentDeviceGamepad();

        switch (currentTutorial)
        {
            case "Jump":
                hintText.text = isGamepad ?
                    LocalizationManager.Get("Press Controller A to jump") :
                    LocalizationManager.Get("Press the space bar to jump!");
                break;
            case "Color":
                hintText.text = isGamepad ?
                    LocalizationManager.Get("Press RB to switch colours") :
                    LocalizationManager.Get("Press C to switch colours!");
                break;
            case "Gravity":
                hintText.text = isGamepad ?
                    LocalizationManager.Get("Press LB to reverse gravity") :
                    LocalizationManager.Get("Press LeftShift to reverse gravity!");
                break;
            default:
                hintText.text = "";
                break;
        }
    }

    bool IsCurrentDeviceGamepad()
    {
        return lastUsedDevice is Gamepad;
    }

    // 退出教程，恢复正常时间，隐藏提示
    void ExitTutorial()
    {
        Time.timeScale = 1f;
        hintText.gameObject.SetActive(false);
        currentTutorial = "";
        lastUsedDevice = null;
    }

    // 实现接口回调

    public void OnJump(InputAction.CallbackContext context)
    {
        if (Time.timeScale < 1f && currentTutorial == "Jump" && context.performed)
            ExitTutorial();

        UpdateLastUsedDevice(context);
    }

    public void OnGravityReverse(InputAction.CallbackContext context)
    {
        if (Time.timeScale < 1f && currentTutorial == "Gravity" && context.performed)
            ExitTutorial();

        UpdateLastUsedDevice(context);
    }

    public void OnChangeColor(InputAction.CallbackContext context)
    {
        if (Time.timeScale < 1f && currentTutorial == "Color" && context.performed)
            ExitTutorial();

        UpdateLastUsedDevice(context);
    }

    public void OnAnybutton(InputAction.CallbackContext context)
    {
        UpdateLastUsedDevice(context);
    }

    // 辅助方法：更新最后使用的设备并刷新提示
    private void UpdateLastUsedDevice(InputAction.CallbackContext context)
    {
        if (context.control != null)
        {
            lastUsedDevice = context.control.device;
            UpdateHintText();
        }
    }
}
