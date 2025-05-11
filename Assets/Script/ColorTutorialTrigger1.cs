using UnityEngine;

public class ColorTutorialTrigger : MonoBehaviour
{
    public TutorialManager tutorialManager; // ��ק��ֵ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager.StartTutorial("Color");
            Destroy(gameObject); // ����������
        }
    }
}