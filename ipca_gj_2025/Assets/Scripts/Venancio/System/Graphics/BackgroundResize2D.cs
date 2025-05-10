using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BackgroundResize2D : MonoBehaviour
{
	[Header("References")]
	[SerializeField, Tooltip("Reference to the CameraData component, which provides camera dimensions and bounds.")]
	private CameraData _camera = null;
	[SerializeField, Tooltip("The Transform to be resized to match the camera's view.")]
	private Transform _transformToResize = null;
	[SerializeField, Tooltip("The SpriteRenderer whose bounds are used to calculate the scaling.")]
	private SpriteRenderer _spriteRenderer = null;



	private void Awake()
	{
		SetupFields();
	}

	void Start()
	{
		Scale();
	}



	private void SetupFields()
	{
		if (!_camera) _camera = GetComponent<CameraData>();
		if (!_camera) Debug.LogError("CameraData reference is missing.", this);

		if (!_transformToResize) _transformToResize = this.transform;
		if (!_transformToResize) Debug.LogError("Transform reference is missing.", this);

		if (!_spriteRenderer) _spriteRenderer = GetComponent<SpriteRenderer>();
		if (!_spriteRenderer) Debug.LogError("SpriteRenderer reference is missing.", this);
	}

	/// <summary>
	/// Scales the transform of the GameObject to match the dimensions of the camera's view.
	/// </summary>
	/// <remarks>
	/// This method adjusts the local scale of the GameObject based on the width and height of the camera's view
	/// and the size of the SpriteRenderer's bounds. It ensures that the GameObject fits perfectly within the
	/// camera's visible area.
	/// </remarks>
	public void Scale()
	{
		Vector3 newScale = transform.localScale;

		newScale.x = _camera.Width / _spriteRenderer.bounds.size.x;
		newScale.y = _camera.Height / _spriteRenderer.bounds.size.y;

		transform.localScale = newScale;
	}
}
