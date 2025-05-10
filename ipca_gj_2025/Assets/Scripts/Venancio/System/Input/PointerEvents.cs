using UnityEngine;
using UnityEngine.Events;

public class PointerEvents : MonoBehaviour
{
	#region Fields

	[Header("References")]
	[SerializeField] private InputManager _inputManager;
    private bool _isPointerOverUI = false;
	[Space, SerializeField] private UnityEvent _onHideChoices;
	[Space]
	[SerializeField] private UnityEvent<Vector2, float> _onPointerDragStart;
	[Space]
	[SerializeField] private UnityEvent<float> _onPointerDragEnd;
	[Space]
	[SerializeField] private UnityEvent<Vector2> _onPointerSingleTap;

	public bool IsPointerOverUI { get => _isPointerOverUI; }
	public UnityEvent OnHideChoices { get => _onHideChoices; }

	private Vector2 _pointerPoistion = Vector2.zero;
	private Vector2 _pointerDirection = Vector2.zero;
	/// <summary>
	/// Gets the current position of the pointer.
	/// </summary>
	public Vector2 PointerPosition { get => _pointerPoistion; }
	/// <summary>
	/// Gets the current direction of the pointer.
	/// </summary>
	public Vector2 PointerDirection { get => _pointerDirection; }
	[Space]
	[SerializeField] private UnityEvent<Vector2> _onPointerDirectionChange;
	[Space]
	[SerializeField] private UnityEvent<RectTransform, HoverMotionType> _onPointerHoverUI;

	[SerializeField] private UnityEvent<Vector2> _onPointerMove;




	/// <summary>
	/// Event triggered when the pointer moves.
	/// </summary>
	/// <remarks>
	/// The Vector2 parameter represents the new position of the pointer.
	/// </remarks>
	public UnityEvent<Vector2> OnPointerMove { get => _onPointerMove; }
	public UnityEvent<RectTransform, HoverMotionType> OnPointerHoverUI { get => _onPointerHoverUI; }



	/// <summary>
	/// Event triggered when the pointer direction changes.
	/// </summary>
	/// <remarks>
	/// The Vector2 parameter represents the new direction of the pointer.
	/// </remarks>
	public UnityEvent<Vector2> OnPointerDirectionChange { get => _onPointerDirectionChange; }

	/// <summary>
	/// Event triggered when the pointer drag action starts.
	/// </summary>
	/// <remarks>
	/// The Vector2 parameter represents the starting position of the pointer.
	/// The float parameter represents the timestamp when the drag action started.
	/// </remarks>
	public UnityEvent<Vector2, float> OnPointerDragStart { get => _onPointerDragStart; }
	/// <summary>
	/// Event triggered when the pointer drag action ends.
	/// </summary>
	public UnityEvent<float> OnPointerDragEnd { get => _onPointerDragEnd; }
	/// <summary>
	/// Event triggered when the pointer is tapped.
	/// </summary>
	/// <remarks>
	/// The Vector2 parameter represents the screen position of the pointer when the tap occurs.
	/// </remarks>
	public UnityEvent<Vector2> OnPointerSingleTap { get => _onPointerSingleTap; }

	#endregion

	private void Awake()
	{
		SetupFields();
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	private void SetupFields()
	{
		if (_onHideChoices == null) _onHideChoices = new UnityEvent();
		if (_onPointerDragStart == null) _onPointerDragStart = new UnityEvent<Vector2, float>();
		if (_onPointerDragEnd == null) _onPointerDragEnd = new UnityEvent<float>();
		if (_onPointerSingleTap == null) _onPointerSingleTap = new UnityEvent<Vector2>();
		if (_onPointerDirectionChange == null) _onPointerDirectionChange = new UnityEvent<Vector2>();
		if (_onPointerHoverUI == null) _onPointerHoverUI = new UnityEvent<RectTransform, HoverMotionType>();
		if (_onPointerMove == null) _onPointerMove = new UnityEvent<Vector2>();
	}

	private void IsHovering()
	{

	}

	private void IsNotHovering()
	{

	}

	#region InputManager events
	public void UpdatePointerPositionChange(Vector2 pointerPosition)
	{
		_pointerPoistion = pointerPosition;
		OnPointerMove.Invoke(pointerPosition);
	}

	public void UpdatePointerDirectionChange(Vector2 pointerDirection)
	{
		_pointerDirection = pointerDirection;
		OnPointerDirectionChange.Invoke(pointerDirection);
	}

	public void OnPointerPressAndDrag(PointerDragState dragState, Vector2 pointerStartingLocation, float timestamp)
	{
		switch (dragState)
		{
			case PointerDragState.Started:
				OnPointerDragStart.Invoke(pointerStartingLocation, timestamp);
				break;

			case PointerDragState.Ended:
				OnPointerDragEnd.Invoke(timestamp);
				break;
		}
	}

	public void OnPointerTap(Vector2 screenPosition)
	{
		OnPointerSingleTap.Invoke(screenPosition);
	}
	#endregion


}