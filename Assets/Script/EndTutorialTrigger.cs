using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndTutorialTrigger : MonoBehaviour
{
    public GameObject endScreen; // ��ק��ֵ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f; // ��ͣ��Ϸ
            endScreen.SetActive(true); // ��ʾ����UI
        }
    }
}