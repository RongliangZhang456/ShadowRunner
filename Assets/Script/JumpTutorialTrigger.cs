using UnityEngine;

public class JumpTutorialTrigger : MonoBehaviour
{
    public TutorialManager tutorialManager; // Assign via drag and drop

	private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager.StartTutorial("Jump");
            Destroy(gameObject); // Destroy after triggering
		}
    }
}