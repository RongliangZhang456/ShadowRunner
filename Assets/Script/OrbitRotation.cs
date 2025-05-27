using UnityEngine;

public class OrbitRotation : MonoBehaviour
{
    public Vector3 orbitAxis = Vector3.up;
    public float orbitSpeed = 20f;

    void Update()
    {
        transform.Rotate(orbitAxis, orbitSpeed * Time.deltaTime);
    }
}
