using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        Debug.Log("Restarting Game..."); // Log restart
		Time.timeScale = 1f; // Restore time
		SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
	}
}