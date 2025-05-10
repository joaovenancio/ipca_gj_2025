//Copyright(C) 2025 Joao Vitor Demaria Venancio under GNU AGPL. Refer to README.md for more information.
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[Header("Optional Reference")]
	[Tooltip("If not manually set, is needed to manually assign this class InputEvents functions to a InputEvents GameObject.")]
	[SerializeField] private PointerEvents _inputEvents;
	[Header("Events")]
	[Space]
	[SerializeField] private UnityEvent _onClick;
	private bool _isPointerOver = false;

	public bool IsInteractable = true;
	/// <summary>
	/// Event triggered when the button is clicked.
	/// </summary>
	public UnityEvent OnClick { get => _onClick; }



	private void OnEnable()
	{
		_inputEvents?.OnPointerSingleTap.AddListener(OnPointerSingleTap);
	}

	private void OnDisable()
	{
		_inputEvents?.OnPointerSingleTap.RemoveListener(OnPointerSingleTap);
	}

	private void Awake()
	{
		SetupFields();
	}



	private void SetupFields()
	{
		if (!_inputEvents) _inputEvents = EventManager2D.Instance.PointerEvents;

		if (_onClick == null) _onClick = new UnityEvent();
	}

	#region InputEvents
	private void OnPointerSingleTap(Vector2 screenPosition)
	{
		if (!IsInteractable) return;
		if (!_isPointerOver) return;
		_onClick.Invoke();
	}

	#endregion

	#region Interfaces
	public void OnPointerEnter(PointerEventData eventData)
	{
		_isPointerOver = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_isPointerOver = false;
	}
	#endregion
}
