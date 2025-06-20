using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SceneInitializer : MonoBehaviour
{
    void Start()
    {
		// 1. Reset skybox and ambient lighting
		RenderSettings.skybox = RenderSettings.skybox;
        DynamicGI.UpdateEnvironment();

		// 2. Ensure the main camera enables depthTexture
		if (Camera.main != null)
        {
            Camera.main.depthTextureMode = DepthTextureMode.Depth;

			// New: Switch the camera's render target to force renderer feature processing
			Camera.main.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
            Camera.main.targetTexture = null;

            Debug.Log("Camera reinitialized for render pipeline");
        }
        else
        {
            Debug.LogWarning("Main Camera not found!");
        }
    }
}
