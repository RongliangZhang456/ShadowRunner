using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SceneInitializer : MonoBehaviour
{
    void Start()
    {
        // 1. 重新设置 skybox 和环境光
        RenderSettings.skybox = RenderSettings.skybox;
        DynamicGI.UpdateEnvironment();

        // 2. 确保主相机开启 depthTexture
        if (Camera.main != null)
        {
            Camera.main.depthTextureMode = DepthTextureMode.Depth;

            //  新增：切换一下相机的渲染目标，强制触发 renderer feature 处理流程
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
