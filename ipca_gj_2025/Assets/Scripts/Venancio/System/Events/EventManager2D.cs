using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-10)]
public class EventManager2D: Singleton<EventManager2D>
{
	[Header("References")]
	[SerializeField] private DialogueEvents _dialogueEvents;
	[SerializeField] private PointerEvents _pointerEvents;

	public DialogueEvents DialogueEvents => _dialogueEvents;
	public PointerEvents PointerEvents => _pointerEvents;

	private void Awake()
	{
		SingletonSetup();
		SetupFields();
	}

	private void SetupFields()
	{
		CreateDialogueEvents();
		CreatePointerEvents();
	}

	private void CreateDialogueEvents()
	{
		if (_dialogueEvents) return;

		GameObject dialogueEventsGameObject = new GameObject("Dialogue Events");
		dialogueEventsGameObject.AddComponent<DialogueEvents>();
		dialogueEventsGameObject.transform.SetParent(this.transform);
		dialogueEventsGameObject.transform.localPosition = Vector3.zero;
	}

	private void CreatePointerEvents()
	{
		if (_pointerEvents) return;

		GameObject pointerEventsGameObject = new GameObject("Pointer Events");
		pointerEventsGameObject.AddComponent<PointerEvents>();
		pointerEventsGameObject.transform.SetParent(this.transform);
		pointerEventsGameObject.transform.localPosition = Vector3.zero;
	}
}