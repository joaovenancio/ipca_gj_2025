using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;
using UnityEngine.Events;
using System;


public class InkManager : MonoBehaviour //TO-DO: Separete the choices logic from the dialogue logic
{
	[Header("References")]
    [SerializeField] private DialogueUI _dialogueUI;
	[SerializeField] private TextAsset _inkJSONAsset = null; 
	[SerializeField] private List<DialogueFunction> _inkFunctionsSetup;
	[SerializeField] private DialogueEvents _dialogueEvents;
	[Header("Settings")]
	private bool _isShowingOnStart = false;

	private Dictionary<string, UnityEvent<string>> _inkFunctions;
	private int _currentChoiceGroup = 0;
	private bool _isDialogueRunning = false;
	private Story _currentStory;



	private void OnEnable()
	{
		_dialogueEvents.OnChooseDialogueOption.AddListener(ChooseDialogueOption);
		_dialogueEvents.OnStartDialogue.AddListener(StartStory);
	}

	private void OnDisable()
	{
		_dialogueEvents.OnChooseDialogueOption.RemoveListener(ChooseDialogueOption);
		_dialogueEvents.OnStartDialogue.RemoveListener(StartStory);
	}

	private void Awake()
	{
		SetupFunctionsDictionary();
	}

	void Start()
    {
		if (_isShowingOnStart) StartStory();
	}



	private void SetupFunctionsDictionary()
	{
		_inkFunctions = new Dictionary<string, UnityEvent<string>>();

		foreach (DialogueFunction function in _inkFunctionsSetup)
		{
			if (function.FunctionEvent == null) continue;
			if (function.FunctionEvent.GetPersistentEventCount() <= 0) continue;
			if (_inkFunctions.ContainsKey(function.FunctionName)) continue;

			_inkFunctions.Add(function.FunctionName, function.FunctionEvent);
		}
	}

	public void SetupFields()
	{
		if (!_dialogueEvents) _dialogueEvents = EventManager2D.Instance.DialogueEvents;
	}

	public void StartStory(TextAsset inkJSONAsset)
	{
		if (_isDialogueRunning) return;

		_isDialogueRunning = true;

		_inkJSONAsset = inkJSONAsset;

		_currentStory = new Story(_inkJSONAsset.text);

		_dialogueEvents.OnDialogueStarted.Invoke();

		RefreshView();
	}
	
	public void StartStory()
	{
		if (_isDialogueRunning) return;

		_isDialogueRunning = true;

		_currentStory = new Story(_inkJSONAsset.text);
		
		_dialogueEvents.OnDialogueStarted.Invoke();

		RefreshView();
	}

	private void SetupDialogueText(string text)
	{
		_dialogueUI.DialogueText.text = text;
	}

	public void RefreshView()
	{

		if (_currentStory.canContinue)
		{
			string text = _currentStory.Continue();
			text = text.Trim();

			ProcessTags(_currentStory.currentTags);
			SetupDialogueText(text);

		} else if (_currentStory.currentChoices.Count > 0)
		{
			SetupChoices();
		}
		else
		{
			EndStory();
		}

	}

	private void ProcessTags(List<string> currentTags)
	{
		foreach (string tag in currentTags)
		{
			string[] keyValuePair = tag.Split(":");

			if (keyValuePair.Length != 2)
			{
				Debug.LogWarning("InkManager: Tag not properly parsed.", this);
				continue;
			}

			string key = keyValuePair[0].Trim();
			string value = keyValuePair[1].Trim();

			if (_inkFunctions.ContainsKey(key))	_inkFunctions[key].Invoke(value);
			else
			{
				Debug.LogWarning($"InkManager: Function not found for tag '{key}'", this);
				continue;
			}
		}
	}

	private void SetupChoices()
	{
		if (_currentStory.currentChoices.Count > _dialogueUI.OptionsButtons.Count)
		{
			Debug.LogError("InkManager: Not enough buttons to display all choices.", this);
			return;
		}

		for (int i = 0; i < _currentStory.currentChoices.Count; i++)
		{
			Choice choice = _currentStory.currentChoices[i];
			ChoiceButton button = _dialogueUI.OptionsButtons[i];

			SetupButtonContent(button, choice);

			button.gameObject.SetActive(true);

		}

		_dialogueEvents.OnShowChoices.Invoke();

	}

	private void SetupButtonContent(ChoiceButton button, Choice choice)
	{
		button.ButtonText.text = choice.text.Trim();
		button.OptionIndex = choice.index;
		button.ChoiceGroup = _currentChoiceGroup;
		button.gameObject.SetActive(true);
	}

	public void ChooseDialogueOption(ChoiceButton option)
	{
		if (option.ChoiceGroup != _currentChoiceGroup) return;

		_currentChoiceGroup++;

		_currentStory.ChooseChoiceIndex(option.OptionIndex);

		_dialogueEvents.OnHideChoices.Invoke();

		RefreshView();
	}

	private void EndStory()
	{
		Debug.LogWarning("END OF STORY");

		_isDialogueRunning = false;

		_dialogueEvents.OnDialogueEnded.Invoke();
	}

}

[Serializable]
public struct DialogueFunction
{
	public string FunctionName;
	public UnityEvent<string> FunctionEvent;
}
