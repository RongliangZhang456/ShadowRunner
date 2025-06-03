using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip jumpClip;
    public AudioClip splashClip;
    public AudioClip colorChangeClip;
    public AudioClip gravityFlipClip;


    // 你新增的音效
    public AudioClip gameOverClip;
    public AudioClip levelCompleteClip;
    public AudioClip collectItemClip;


    void Start()
    {
        // 自动获取 audioSource（更保险）
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // 各种播放函数（统一加空检查，避免播放空音频）

    public void PlayJumpSound()
    {
        PlayOneShotSafe(jumpClip);
    }

    public void PlaySplashSound()
    {
        PlayOneShotSafe(splashClip);
    }

    public void PlayColorChangeSound()
    {
        PlayOneShotSafe(colorChangeClip);
    }

    public void PlayGravityFlipSound()
    {
        PlayOneShotSafe(gravityFlipClip);
    }

    public void PlayGameOverSound()
    {
        PlayOneShotSafe(gameOverClip);
    }

    public void PlayLevelCompleteSound()
    {
        PlayOneShotSafe(levelCompleteClip);
    }

    public void PlayCollectItemSound()
    {
        PlayOneShotSafe(collectItemClip);
    }

    // 公共播放逻辑封装
    private void PlayOneShotSafe(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("音效播放失败，AudioSource 或 Clip 未设置！");
        }
    }
}