using UnityEngine;

public class NextDialogue : MonoBehaviour
{
    [SerializeField] private DialogueEvents _dialogueEvents;

	private void Awake()
	{
		SetupFields();
	}

	private void SetupFields()
	{
		if (_dialogueEvents == null) _dialogueEvents = EventManager2D.Instance.DialogueEvents;
	}

	public void Play()
    {
        _dialogueEvents.OnNext.Invoke();
	}
}
