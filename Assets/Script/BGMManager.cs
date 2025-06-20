using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioSource bgmSource;
    public AudioClip menuBGM;
    public AudioClip level1BGM;
    public AudioClip level2BGM;
    public AudioClip level3BGM;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Do not destroy on scene load
		}
        else
        {
            Destroy(gameObject); // Prevent duplicates
		}
    }

    public void PlayMenuBGM()
    {
        PlayBGM(menuBGM);
    }

    public void PlayLevelBGM(int level)
    {
        switch (level)
        {
            case 1: PlayBGM(level1BGM); break;
            case 2: PlayBGM(level2BGM); break;
            case 3: PlayBGM(level3BGM); break;
            default: bgmSource.Stop(); break;
        }
    }

    private void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}
