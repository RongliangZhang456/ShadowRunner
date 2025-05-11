using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        Debug.Log("Restarting Game..."); // 输出日志
        Time.timeScale = 1f; // 恢复时间
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 重新加载当前场景
    }
}