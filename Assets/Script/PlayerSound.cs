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


	// Additional sound effects
	public AudioClip gameOverClip;
    public AudioClip levelCompleteClip;
    public AudioClip collectItemClip;


    void Start()
    {
		// Automatically get audioSource (safer)
		if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

	// Various play functions (with null checks to avoid playing empty clips)

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

	// Common play logic encapsulation
	private void PlayOneShotSafe(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Failed to play sound effect, AudioSource or Clip not set!");
        }
    }
}