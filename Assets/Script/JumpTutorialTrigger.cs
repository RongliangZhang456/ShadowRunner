using UnityEngine;

public class JumpTutorialTrigger : MonoBehaviour
{
    public TutorialManager tutorialManager; // 拖拽赋值

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager.StartTutorial("Jump");
            Destroy(gameObject); // 触发后销毁
        }
    }
}