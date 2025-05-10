using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private PointerEvents _pointerEvents;
	[SerializeField] private DialogueEvents _dialogueEvents;

	private bool _isDialogueRunning = false;
	private bool _isPointerOverUI = false;

	private void OnEnable()
	{
		_dialogueEvents.OnDialogueStarted.AddListener(OnDialogueStarted);
		_dialogueEvents.OnDialogueEnded.AddListener(OnDialogueEnded);
		_pointerEvents.OnPointerHoverUI.AddListener(OnPointerHoverUI);
		_pointerEvents.OnPointerSingleTap.AddListener(OnPointerClick);
	}

	private void OnDisable()
	{
		_dialogueEvents.OnDialogueStarted.RemoveListener(OnDialogueStarted);
		_dialogueEvents.OnDialogueEnded.RemoveListener(OnDialogueEnded);
		_pointerEvents.OnPointerHoverUI.RemoveListener(OnPointerHoverUI);
		_pointerEvents.OnPointerSingleTap.RemoveListener(OnPointerClick);
	}

	private void Awake()
	{
		SetupFields();
	}


	private void OnPointerHoverUI(RectTransform rect, HoverMotionType motionType)
	{
		switch (motionType)
		{
			case HoverMotionType.Enter:
				_isPointerOverUI = true;
				break;

			case HoverMotionType.Exit:
				_isPointerOverUI = false;
				break;
		}
	}

	private void OnPointerClick(Vector2 position)
	{
		if (_isPointerOverUI || !_isDialogueRunning) return;

		_dialogueEvents.OnNext.Invoke();
	}

	private void OnDialogueStarted(TextAsset textAsset)
	{
		_isDialogueRunning = true;
	}


	private void OnDialogueEnded()
	{
		_isDialogueRunning = false;
	}
	private void SetupFields()
	{
		if (!_pointerEvents) _pointerEvents = EventManager2D.Instance.PointerEvents;
		if (!_dialogueEvents) _dialogueEvents = EventManager2D.Instance.DialogueEvents;
	}

}
