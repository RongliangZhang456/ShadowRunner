using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndTutorialTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f; // ��ͣ��Ϸ
            UnityEngine.SceneManagement.SceneManager.LoadScene("YouWinScene");
        }
    }
}