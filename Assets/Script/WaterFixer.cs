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
        yield return null; // 等待一帧确保相机初始化完成

        Camera cam = Camera.main;
        if (cam != null)
        {
            RenderTexture tempRT = new RenderTexture(Screen.width, Screen.height, 24);
            cam.targetTexture = tempRT;

            yield return null; // 再等一帧，确保渲染器感知变化

            cam.targetTexture = null; // 恢复默认渲染目标
            Debug.Log(" 相机渲染目标已重置，触发渲染器刷新");
        }
        else
        {
            Debug.LogWarning(" 没有找到 Main Camera");
        }
    }
}
