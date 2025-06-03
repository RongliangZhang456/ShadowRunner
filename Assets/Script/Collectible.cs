using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject onControllerEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("collusion");

            // 播放收集音效
            PlayerSound sound = other.GetComponent<PlayerSound>();
            if (sound != null)
            {
                sound.PlayCollectItemSound();
            }

            if (onControllerEffect != null)
                Instantiate(onControllerEffect, transform.position, transform.rotation);
            else
                Debug.LogWarning("onControllerEffect is null!");

            if (GameStatsManager.Instance != null)
                GameStatsManager.Instance.AddStar();
            else
                Debug.LogWarning("GameStatsManager.Instance is null!");

            // Destroy the Collectible
            Destroy(gameObject);
        }
    }
}
