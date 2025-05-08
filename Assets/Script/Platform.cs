using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool isBlackPlatform = true;

    void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            // ֱ�ӷ���public�ֶ�
            bool shouldCollide = (isBlackPlatform && player.isBlack) ||
                                (!isBlackPlatform && !player.isBlack);

            GetComponent<Collider>().isTrigger = !shouldCollide;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        GetComponent<Collider>().isTrigger = false;
    }
}