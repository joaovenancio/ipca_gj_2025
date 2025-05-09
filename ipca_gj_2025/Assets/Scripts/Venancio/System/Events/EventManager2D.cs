using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-10)]
public class EventManager2D: Singleton<EventManager2D>
{
	[Header("References")]
	[SerializeField] private DialogueEvents _dialogueEvents;
	public DialogueEvents DialogueEvents => _dialogueEvents;

	private void Awake()
	{
		SingletonSetup();
		SetupFields();
	}

	private void SetupFields()
	{
		CreateDialogueEvents();
	}

	private void CreateDialogueEvents()
	{
		if (_dialogueEvents) return;

		GameObject dialogueEventsGameObject = new GameObject("Dialogue Events");
		dialogueEventsGameObject.AddComponent<DialogueEvents>();
		dialogueEventsGameObject.transform.SetParent(this.transform);
		dialogueEventsGameObject.transform.localPosition = Vector3.zero;
	}
}