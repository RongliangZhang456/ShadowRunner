using UnityEngine;

public class GravityTutorialTrigger : MonoBehaviour
{
    public TutorialManager tutorialManager; // Assign via drag and drop

	private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager.StartTutorial("Gravity");
            Destroy(gameObject); // Destroy after triggering
		}
    }
}