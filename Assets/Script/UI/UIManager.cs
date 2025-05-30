using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private Dictionary<string, PanelBase> panelMap = new Dictionary<string, PanelBase>();

    private void Awake()
    {
        LocalizationManager.LoadLanguage("English");
        Instance = this;
        RegisterAllPanels();
    }

    private void RegisterAllPanels()
    {
        Debug.Log("Registering all panels...");
        PanelBase[] panels = FindObjectsOfType<PanelBase>(true);
        panelMap.Clear();
        foreach (var panel in panels)
        {
            panelMap.Add(panel.PanelID, panel);
        }
    }

    public void ShowPanel(string panelID)
    {
        if (panelMap.TryGetValue(panelID, out var panel))
        {
            panel.Show();
        }
        else
        {
            Debug.LogWarning($"Panel ID '{panelID}' not found.");
        }
    }

    public void HidePanel(string panelID)
    {
        if (panelMap.TryGetValue(panelID, out var panel))
        {
            panel.Hide();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RegisterAllPanels();
    }
}