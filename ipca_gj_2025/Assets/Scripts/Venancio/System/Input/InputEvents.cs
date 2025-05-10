//Copyright(C) 2025 João Vítor Demaria Venâncio under GNU AGPL. Refer to README.md for more information.
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-10)]
public class InputEvents : MonoBehaviour
{
	private Vector2 _pointerPoistion = Vector2.zero;
	private Vector2 _pointerDirection = Vector2.zero;
	[Header("Public Events")]
	[Space]
	[SerializeField] private UnityEvent<Vector2, float> _onDragNothing;
	[Space]
	[SerializeField] private UnityEvent<Vector2, float> _onPointerDragStart;
	[Space]
	[SerializeField] private UnityEvent<float> _onPointerDragEnd;
	[Space]
	[SerializeField] private UnityEvent<Vector2> _onPointerSingleTap;
	[Space]
	[SerializeField] private UnityEvent<Vector2> _onPointerMove;
	[Space]
	[SerializeField] private UnityEvent<Vector2> _onPointerDirectionChange;
	[Space]
	[SerializeField] private UnityEvent<RectTransform, HoverMotionType> _onPointerHoverUI;

	/// <summary>
	/// Gets the current position of the pointer.
	/// </summary>
	public Vector2 PointerPosition { get => _pointerPoistion; }
	/// <summary>
	/// Gets the current direction of the pointer.
	/// </summary>
	public Vector2 PointerDirection { get => _pointerDirection; }
	/// <summary>
	/// Event triggered when a drag action occurs without any specific target.
	/// </summary>
	/// <remarks>
	/// The Vector2 parameter represents the position of the pointer during the drag action.
	/// </remarks>
	public UnityEvent<Vector2, float> OnDragNothing { get => _onDragNothing; }
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
	/// <summary>
	/// Event triggered when the pointer moves.
	/// </summary>
	/// <remarks>
	/// The Vector2 parameter represents the new position of the pointer.
	/// </remarks>
	public UnityEvent<Vector2> OnPointerMove { get => _onPointerMove; }
	/// <summary>
	/// Event triggered when the pointer direction changes.
	/// </summary>
	/// <remarks>
	/// The Vector2 parameter represents the new direction of the pointer.
	/// </remarks>
	public UnityEvent<Vector2> OnPointerDirectionChange { get => _onPointerDirectionChange; }
	/// <summary>
	/// Event triggered when the pointer hovers over a UI element.
	/// </summary>
	/// <remarks>
	/// The RectTransform parameter represents the UI element being hovered over.
	/// The HoverMotionType parameter indicates whether the pointer is entering or exiting the UI element.
	/// </remarks>
	public UnityEvent<RectTransform, HoverMotionType> OnPointerHoverUI { get => _onPointerHoverUI; }
	


	private void Awake()
	{
		SetupFields();
	}



	private void SetupFields()
	{
		if (_onDragNothing == null) _onDragNothing = new UnityEvent<Vector2, float>();
		if (_onPointerDragStart == null) _onPointerDragStart = new UnityEvent<Vector2, float>();
		if (_onPointerDragEnd == null) _onPointerDragEnd = new UnityEvent<float>();
		if (_onPointerSingleTap == null) _onPointerSingleTap = new UnityEvent<Vector2>();
		if (_onPointerMove == null) _onPointerMove = new UnityEvent<Vector2>();
		if (_onPointerDirectionChange == null) _onPointerDirectionChange = new UnityEvent<Vector2>();
		if (_onPointerHoverUI == null) _onPointerHoverUI = new UnityEvent<RectTransform, HoverMotionType>();
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
		switch(dragState)
		{
			case PointerDragState.Started:
				OnPointerDragStart.Invoke(pointerStartingLocation, timestamp);
				break;

			case PointerDragState.Ended:
				OnPointerDragEnd.Invoke(timestamp);
				break;
		}
	}

	public void OnPointerTap (Vector2 screenPosition)
	{
		OnPointerSingleTap.Invoke(screenPosition);
	}
	#endregion
}

public enum HoverMotionType
{
	Enter,
	Exit
}