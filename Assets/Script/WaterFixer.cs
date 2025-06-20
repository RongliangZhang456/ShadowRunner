using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WaterFixer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ResetCameraRenderTarget());
    }

    System.Collections.IEnumerator ResetCameraRenderTarget()
    {
        yield return null; // Wait one frame to ensure the camera is initialized

		Camera cam = Camera.main;
        if (cam != null)
        {
            RenderTexture tempRT = new RenderTexture(Screen.width, Screen.height, 24);
            cam.targetTexture = tempRT;

            yield return null; // Wait another frame to ensure the renderer detects the change

			cam.targetTexture = null; // Restore the default render target
			Debug.Log("Camera render target has been reset, renderer refresh triggered");
        }
        else
        {
            Debug.LogWarning("Main Camera not found");
        }
    }
}
