using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool isBlackPlatform = true;

    void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            GetComponent<Collider>().isTrigger = (isBlackPlatform != player.isBlack);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        GetComponent<Collider>().isTrigger = false;
    }
}