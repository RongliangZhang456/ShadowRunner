using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class LocalizationEntry
{
    public string key;
    public string value;
}

[Serializable]
public class LocalizationData
{
    public List<LocalizationEntry> entries;
}

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }
    private static Dictionary<string, string> localizedWords = new Dictionary<string, string>();

    public static string CurrentLanguage { get; private set; } = "English";
    private void Awake()
    {
        Instance = this;
    }

    public static void LoadLanguage(string language)
    {
        language = char.ToUpper(language[0]) + language.Substring(1).ToLower();
        string path = Path.Combine(Application.dataPath, "Dictionary", $"{language}.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LocalizationData data = JsonUtility.FromJson<LocalizationData>(json);

            localizedWords.Clear();
            foreach (var entry in data.entries)
            {
                localizedWords[entry.key] = entry.value;
            }

            CurrentLanguage = language;

        }
        else
        {
            Debug.LogWarning("Language file not found: " + path);
        }
    }

    public static string Get(string key)
    {
        if (localizedWords.TryGetValue(key, out string value))
        {
            return value;
        }
        return key; // fallback: return key itself if not found
    }
}