using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        Debug.Log("Restarting Game..."); // �����־
        Time.timeScale = 1f; // �ָ�ʱ��
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���¼��ص�ǰ����
    }
}