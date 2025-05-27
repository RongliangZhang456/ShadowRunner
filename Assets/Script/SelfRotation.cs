using UnityEngine;

public class SelfRotation : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 30f; // Ã¿ÃëÐý×ª½Ç¶È

    void Update()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
