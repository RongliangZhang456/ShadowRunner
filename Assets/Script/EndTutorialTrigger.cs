using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndTutorialTrigger : MonoBehaviour
{
    public GameObject endScreen; // 拖拽赋值

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f; // 暂停游戏
            endScreen.SetActive(true); // 显示结束UI
        }
    }
}