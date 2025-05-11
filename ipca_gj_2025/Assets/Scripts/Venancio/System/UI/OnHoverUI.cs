//Copyright(C) 2025 Joao Vitor Demaria Venancio under GNU AGPL. Refer to README.md for more information.
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Manages UI hover events, triggering enter and exit events when the pointer interacts with the UI element.
/// </summary>
public class OnHoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[Header("Optional Reference")]
	[SerializeField] private RectTransform _rectTransform;
	[SerializeField] private PointerEvents _pointerEvents;
	[Header("Events")]
	[SerializeField] private UnityEvent<RectTransform> _onEnter;
	[SerializeField] private UnityEvent<RectTransform> _onExit;

	public bool _isPointerOver = false;

	public UnityEvent<RectTransform> OnEnter { get => _onEnter; }
	public UnityEvent<RectTransform> OnExit { get => _onExit; }
	public bool IsPointerOver { get => _isPointerOver; } 

	public bool isExitingOnDisable = false;

	private void OnEnable()
	{

	}

	private void OnDisable()
	{
		if(isExitingOnDisable)
			ExitUI();
    }

	private void Awake()
	{
		SetupFields();
	}

	public void StopHovering()
	{
		ExitUI();
	}

	private void SetupFields()
	{
		if (!_rectTransform) _rectTransform = this.GetComponent<RectTransform>();
		if (!_rectTransform) _rectTransform = this.GetComponentInChildren<RectTransform>();
		if (!_rectTransform) Debug.LogWarning("No RectTransform found in " + this.gameObject.name + " or its children.");

		if (!_pointerEvents) _pointerEvents = EventManager2D.Instance.PointerEvents;
	}

	/// <summary>
	/// Triggers the OnExit event and invokes the OnPointerHoverUI event.
	/// </summary>
	private void ExitUI()
	{
		OnExit.Invoke(_rectTransform);

		_pointerEvents?.OnPointerHoverUI.Invoke(_rectTransform, HoverMotionType.Exit);
	}

	#region Interfaces
	/// <summary>
	/// Handles the pointer entering the UI element.
	/// </summary>
	/// <remarks>
	/// Sets the _isPointerOver flag to true and triggers the OnEnter event if the UI is not being dragged.
	/// </remarks>
	public void OnPointerEnter(PointerEventData eventData)
	{
		_isPointerOver = true;

		_pointerEvents?.OnPointerHoverUI.Invoke(_rectTransform, HoverMotionType.Enter);
		OnEnter.Invoke(_rectTransform);
	}

	/// <summary>
	/// Handles the pointer exiting the UI element.
	/// </summary>
	/// <remarks>
	/// Sets the _isPointerOver flag to false and triggers the OnExit event if the UI is not being dragged.
	/// </remarks>
	public void OnPointerExit(PointerEventData eventData)
	{	
		_isPointerOver = false;

		ExitUI();
	}
	#endregion
}
