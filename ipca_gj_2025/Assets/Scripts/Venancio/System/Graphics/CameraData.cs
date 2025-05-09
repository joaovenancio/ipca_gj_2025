using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraData : MonoBehaviour
{
	#region Fields
	[Header("References")]
	[SerializeField] private Camera _camera = null;

	private float _leftBound = 0f;
	private float _rightBound = 0f;
	private float _topBound = 0f;
	private float _bottomBound = 0f;
	private float _topRight = 0f;
	private float _bottomLeft = 0f;
	private float _width = 0f;
	private float _height = 0f;

	[Header("Variables")]
	public bool IsUpdatingVariables = false;

	public float LeftBound { get => _leftBound; private set => _leftBound = value; }
	public float RightBound { get => _rightBound; private set => _rightBound = value; }
	public float TopBound { get => _topBound; private set => _topBound = value; }
	public float BottomBound { get => _bottomBound; private set => _bottomBound = value; }
	public float TopRight { get => _topRight; private set => _topRight = value; }
	public float BottomLeft { get => _bottomLeft; private set => _bottomLeft = value; }
	public float Width { get => _width; private set => _width = value; }
	public float Height { get => _height; private set => _height = value; }
	public Camera Camera
	{
		get => _camera;
		set
		{
			_camera = value;
			Awake();
		}
	}
	#endregion



	#region Unity Messeges
	private void Awake()
	{
		if (!Camera) this.GetComponent<Camera>();
		if (!Camera) Debug.LogError("No camera found in the GameObject.");

		CalculateBounds();
		CalculateWidthHeight();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		Debug.Log($"LeftBound: {LeftBound}");
		Debug.Log($"RightBound: {RightBound}");
		Debug.Log($"TopBound: {TopBound}");
		Debug.Log($"BottomBound: {BottomBound}");
	}

	// Update is called once per frame
	void Update()
	{
		if (!IsUpdatingVariables) return;

		CalculateBounds();
		CalculateWidthHeight();

	}
	#endregion



	/// <summary>
	/// Calculates the bounds of the camera's view frustum and updates the left, right, top, and bottom bounds.
	/// </summary>
	private void CalculateBounds()
	{
		if (!Camera) return;

		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main); //Ordering: [0] = Left, [1] = Right, [2] = Down, [3] = Up, [4] = Near, [5] = Far

		_leftBound = planes[0].distance * -1;
		_rightBound = planes[1].distance;
		_bottomBound = planes[2].distance * -1;
		_topBound = planes[3].distance;
	}

	/// <summary>
	/// Calculates the width and height of the camera's view based on its orthographic size and aspect ratio.
	/// </summary>
	private void CalculateWidthHeight()
	{
		_height = _camera.orthographicSize * 2f;
		_width = _camera.aspect * _height;
	}
}
