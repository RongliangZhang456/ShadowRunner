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

    void Start()
    {
        // 确保 audioSource 是当前物体上的组件
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpClip);
    }

    public void PlaySplashSound()
    {
        audioSource.PlayOneShot(splashClip);
    }

    public void PlayColorChangeSound()
    {
        audioSource.PlayOneShot(colorChangeClip);
    }

    public void PlayGravityFlipSound()
    {
        audioSource.PlayOneShot(gravityFlipClip);
    }
}
