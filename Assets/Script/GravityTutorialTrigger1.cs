using UnityEngine;

public class GravityTutorialTrigger : MonoBehaviour
{
    public TutorialManager tutorialManager; // ��ק��ֵ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager.StartTutorial("Gravity");
            Destroy(gameObject); // ����������
        }
    }
}