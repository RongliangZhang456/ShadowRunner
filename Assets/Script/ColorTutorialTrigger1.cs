using UnityEngine;

public class ColorTutorialTrigger : MonoBehaviour
{
    public TutorialManager tutorialManager; // Assign via drag and drop

	private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager.StartTutorial("Color");
            Destroy(gameObject); // Destroy after triggering
		}
    }
}