using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance { get; private set; }

    public float playTime = 0f;
    public int starsCollected = 0;
    public int restartCount = 0;
    private bool isTracking = false;

    public float PlayTime => playTime;
    public int StarsCollected => starsCollected;
    public int RestartCount => restartCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (isTracking)
        {
            playTime += Time.deltaTime;
        }
    }

    public void AddStar(int count = 1)
    {
        starsCollected += count;
    }

    public void RestartStats()
    {
        playTime = 0f;
        restartCount++;
        starsCollected = 0;
        isTracking = true;
    }

    public void PauseTracking()
    {
        isTracking = false;
    }

    public void ResumeTracking()
    {
        isTracking = true;
    }

    public void ResetStats()
    {
        playTime = 0f;
        starsCollected = 0;
        restartCount = 0;
        isTracking = true;
    }
}