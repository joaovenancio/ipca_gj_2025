using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    [Header("References")]
	[SerializeField] private Button _button;
	[SerializeField] private TMPro.TMP_Text _buttonText;
    [Header("Option Reference")]
	[SerializeField] private DialogueEvents _dialogueEvents;

    [Header("Variables")]
    public int Option = 0;
    public int ChoiceIndex = 0;
	public TMPro.TMP_Text ButtonText => _buttonText;

	private void Awake()
	{
		SetupFields();
		SetupButton();
	}

	// Update is called once per frame
	void Update()
    {
        
    }


	private void SetupFields()
	{
		if (!_button) _button = GetComponent<Button>();
		if (!_button) _button = GetComponentInChildren<Button>();
		if (!_button)
		{
			Debug.LogError("ChoiceButton: No Button component found.");
			this.enabled = false;
			return;
		}

		if (_buttonText) _buttonText = GetComponentInChildren<TMPro.TMP_Text>();

		if (!_dialogueEvents) _dialogueEvents = EventManager2D.Instance.DialogueEvents;
		if (!_dialogueEvents)
		{
			Debug.LogError("ChoiceButton: No DialogueEvents component found.");
			this.enabled = false;
			return;
		}
	}


	public void SetupButton()
	{
		_button.onClick.RemoveAllListeners();
		_button.onClick.AddListener(OnClickButton);
	}

	public void OnClickButton()
    {
		_dialogueEvents.OnChooseDialogueOption.Invoke(this);
	}
}
