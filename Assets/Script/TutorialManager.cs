using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour, PlayerControls.IGameplayActions
{
    public TextMeshProUGUI hintText;
    public float slowMotionScale = 0.3f;

    private PlayerControls controls;
    private string currentTutorial = "";

    private InputDevice lastUsedDevice = null; // Record the last used device

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

	// Start the tutorial, enable slow motion, and show the hint
	public void StartTutorial(string type)
    {
        currentTutorial = type;
        UpdateHintText();

        Time.timeScale = slowMotionScale;
        hintText.gameObject.SetActive(true);
    }

	// Update the hint text, support multiple languages and gamepad/keyboard switching
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

	// Exit the tutorial, restore normal time, and hide the hint
	void ExitTutorial()
    {
        Time.timeScale = 1f;
        hintText.gameObject.SetActive(false);
        currentTutorial = "";
        lastUsedDevice = null;
    }

	// Implement interface callbacks
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

	// Helper method: update the last used device and refresh the hint
	private void UpdateLastUsedDevice(InputAction.CallbackContext context)
    {
        if (context.control != null)
        {
            lastUsedDevice = context.control.device;
            UpdateHintText();
        }
    }
}
