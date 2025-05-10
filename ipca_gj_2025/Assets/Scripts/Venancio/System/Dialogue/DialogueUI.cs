using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
	[Header("Referencess")]
	[SerializeField] private GameObject _dialoguePanel;
	[SerializeField] private TMPro.TMP_Text _dialogueText;
	[SerializeField] private TMPro.TMP_Text _speakerName;
	[SerializeField] private GameObject _choicesPanel;
	[SerializeField] private List<ChoiceButton> _choiceButtons;
	[SerializeField] private DialogueEvents _dialogueEvents;

	/// <summary>
	/// Gets the dialogue panel GameObject, which represents the UI container for dialogue.
	/// </summary>
	public GameObject DialoguePanel => _dialoguePanel;
	/// <summary>
	/// Gets the TMP_Text component used to display the dialogue text.
	/// </summary>
	public TMP_Text DialogueText => _dialogueText;
	/// <summary>
	/// Gets the TMP_Text component used to display the speaker's name.
	/// </summary>
	public TMP_Text SpeakerName => _speakerName;
	/// <summary>
	/// Gets the list of ChoiceButton components, representing the available dialogue options.
	/// </summary>
	public List<ChoiceButton> OptionsButtons => _choiceButtons;
	/// <summary>
	/// Gets the choices panel GameObject, which contains the UI for displaying dialogue options.
	/// </summary>
	public GameObject OptionsPanel => _choicesPanel;



	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Awake()
	{
		SetupFields();
		HideChoices();
		HideDialogueBox();
	}



	private void SetupFields()
	{
		if (_dialoguePanel == null)
		{
			Debug.LogError("DialogueUI: No Dialogue Panel assigned.");
			this.enabled = false;
			return;
		}

		if (_dialogueText == null)
		{
			Debug.LogError("DialogueUI: No Dialogue Text assigned.");
			this.enabled = false;
			return;
		}

		if (_speakerName == null)
		{
			Debug.LogError("DialogueUI: No Speaker Name assigned.");
			this.enabled = false;
			return;
		}

		if (_choicesPanel == null)
		{
			Debug.LogError("DialogueUI: No Choices Panel assigned.");
			this.enabled = false;
			return;
		}

		if (_choiceButtons == null)
		{
			Debug.LogError("DialogueUI: No Choice Buttons assigned.");
			this.enabled = false;
			return;
		}

		if (!_dialogueEvents) _dialogueEvents = EventManager2D.Instance.DialogueEvents;
		if (!_dialogueEvents)
		{
			Debug.LogError("DialogueUI: No Dialogue Events assigned.");
			this.enabled = false;
			return;
		}

	}

	public void ShowChoices() 
	{
		_choicesPanel.SetActive(true);
	}

	public void HideChoices()
	{
		_choicesPanel.SetActive(false);
		SetDefaultValuesForChoicesButtons();
	}

	public void ShowDialogueBox()
	{
		_dialoguePanel.SetActive(true);
	}

	public void HideDialogueBox()
	{
		_dialoguePanel.SetActive(false);
		_speakerName.text = "";
		_dialogueText.text = "";
	}

	public void SetDefaultValuesForChoicesButtons()
    {
		foreach (ChoiceButton button in _choiceButtons)
        {
            button.OptionIndex = 0;
            button.ChoiceGroup = 0;
			button.ButtonText.text = "";
			button.gameObject.SetActive(false);
		}

	}

}
