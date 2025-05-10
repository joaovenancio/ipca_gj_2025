using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    [Header("References")]
	[SerializeField] private Button _button;
	[SerializeField] private TMPro.TMP_Text _buttonText;
    [Header("Optional Reference")]
	[SerializeField] private DialogueEvents _dialogueEvents;

	/// <summary>
	/// The index of the option this button represents in the dialogue system.
	/// </summary>
	public int OptionIndex = 0;
	/// <summary>
	/// The group this choice belongs to, used to check if this Choice is part of the current group of choices and can be selected.
	/// </summary>
	public int ChoiceGroup = 0;

	/// <summary>
	/// Provides access to the text component of the button, used to display the dialogue option text.
	/// </summary>
	public TMPro.TMP_Text ButtonText => _buttonText;



	private void Awake()
	{
		SetupFields();
		SetupButton();
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
