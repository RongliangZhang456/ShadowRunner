using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneMaterialSettings : MonoBehaviour
{
	[Min(0.1f)]
	public float textureScale = 10.0f;

	// Start is called before the first frame update
	void Start()
    {
		SetTextureTiling();
	}

	void OnValidate()
	{
		textureScale = Mathf.Max(0.1f, textureScale);
	}

	private void SetTextureTiling()
	{
		MeshRenderer renderer = GetComponent<MeshRenderer>();

		Vector3 scale = transform.localScale;
		Vector2 tiling = new Vector2(scale.x / textureScale, scale.z / textureScale); // Set the tiling based on the scale of the object

		renderer.material.SetTextureScale("_BaseMap", tiling);
	}
}
