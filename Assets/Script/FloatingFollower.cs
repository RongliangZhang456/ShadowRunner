using UnityEngine;

public class FloatingXYOrbit : MonoBehaviour
{
    public Transform target;              // Main character
	public Vector3 offset = new Vector3(2f, 0f, 0f); // Offset from center
	public float orbitRadius = 0.5f;      // Orbit radius
	public float orbitSpeed = 1f;         // Orbit speed
	public float noiseScale = 0.3f;       // Noise disturbance amount
	public float noiseSpeed = 0.5f;       // Noise speed

	void Update()
    {
        if (target == null) return;

		// Orbit center: main character + offset
		Vector3 center = target.position + offset;

		// Time-based angle
		float angle = Time.time * orbitSpeed;

		// Orbit path on the XY plane
		float x = Mathf.Cos(angle) * orbitRadius;
        float y = Mathf.Sin(angle) * orbitRadius;

		// Add slight noise disturbance
		float noiseX = (Mathf.PerlinNoise(Time.time * noiseSpeed, 0f) - 0.5f) * noiseScale;
        float noiseY = (Mathf.PerlinNoise(0f, Time.time * noiseSpeed) - 0.5f) * noiseScale;

        Vector3 orbitOffset = new Vector3(x + noiseX, y + noiseY, 0f); // Z=0£¬XY plane

		transform.position = center + orbitOffset;
    }
}
