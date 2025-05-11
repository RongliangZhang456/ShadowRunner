using UnityEngine;

public class JumpTutorialTrigger : MonoBehaviour
{
    public TutorialManager tutorialManager; // ��ק��ֵ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager.StartTutorial("Jump");
            Destroy(gameObject); // ����������
        }
    }
}