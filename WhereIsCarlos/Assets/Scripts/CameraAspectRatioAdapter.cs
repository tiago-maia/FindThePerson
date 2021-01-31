using UnityEngine;

public class CameraAspectRatioAdapter : MonoBehaviour
{
	new Camera camera;
	float originalOrthographicSize;

	void Awake()
	{
		camera = GetComponent<Camera>();
		originalOrthographicSize = camera.orthographicSize;
	}

	void Update()
	{
		float aspectRatio = Screen.width / (float) Screen.height;
		aspectRatio = Mathf.Min(aspectRatio, 16f/9f);
		camera.orthographicSize = originalOrthographicSize / (aspectRatio / (16f/9f));
	}
}
