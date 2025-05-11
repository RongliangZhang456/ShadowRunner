using UnityEngine;

public class ColorTutorialTrigger : MonoBehaviour
{
    public TutorialManager tutorialManager; // 拖拽赋值

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager.StartTutorial("Color");
            Destroy(gameObject); // 触发后销毁
        }
    }
}