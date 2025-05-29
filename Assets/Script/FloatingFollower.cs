using UnityEngine;

public class FloatingXYOrbit : MonoBehaviour
{
    public Transform target;              // 主角
    public Vector3 offset = new Vector3(2f, 0f, 0f); // 偏移中心点
    public float orbitRadius = 0.5f;      // 环绕半径
    public float orbitSpeed = 1f;         // 环绕速度
    public float noiseScale = 0.3f;       // 噪声扰动大小
    public float noiseSpeed = 0.5f;       // 噪声速度

    void Update()
    {
        if (target == null) return;

        // 环绕中心点：主角 + 偏移
        Vector3 center = target.position + offset;

        // 时间角度
        float angle = Time.time * orbitSpeed;

        // XY 平面上的环绕轨迹
        float x = Mathf.Cos(angle) * orbitRadius;
        float y = Mathf.Sin(angle) * orbitRadius;

        // 加入轻微的噪声扰动
        float noiseX = (Mathf.PerlinNoise(Time.time * noiseSpeed, 0f) - 0.5f) * noiseScale;
        float noiseY = (Mathf.PerlinNoise(0f, Time.time * noiseSpeed) - 0.5f) * noiseScale;

        Vector3 orbitOffset = new Vector3(x + noiseX, y + noiseY, 0f); // Z=0，XY 平面

        transform.position = center + orbitOffset;
    }
}
