using UnityEngine;
using UnityEngine.Events;

public class DialogueEvents : MonoBehaviour
{
	#region Fields
	[Header("Public Events")]
	[Tooltip("Event triggered when a dialogue option is chosen. Parameters: ChoiceButton and option index.")]
	[Space, SerializeField] private UnityEvent<ChoiceButton> _onChooseDialogueOption;
	[Tooltip("Event triggered when the next dialogue text is shown. Parameter: The text to display.")]
	[Space, SerializeField] private UnityEvent<string> _onShowNextText;
	[Tooltip("Event triggered when dialogue choices are displayed.")]
	[Space, SerializeField] private UnityEvent _onShowChoices;
	[Tooltip("Event triggered when dialogue choices are hidden.")]
	[Space, SerializeField] private UnityEvent _onHideChoices;
	[Tooltip("Event triggered when the dialogue ends.")]
	[Space, SerializeField] private UnityEvent _onDialogueEnded;
	[Tooltip("Event triggered when the dialogue starts.")]
	[Space, SerializeField] private UnityEvent _onDialogueStarted;
	[Tooltip("Event triggered to start an dialogue.")]
	[Space, SerializeField] private UnityEvent<TextAsset> _onStartDialogue;


	public UnityEvent<ChoiceButton> OnChooseDialogueOption { get => _onChooseDialogueOption; }
	public UnityEvent<string> OnShowNextText { get => _onShowNextText; }
	public UnityEvent OnShowChoices { get => _onShowChoices; }
	public UnityEvent OnHideChoices { get => _onHideChoices; }
	public UnityEvent OnDialogueEnded { get => _onDialogueEnded; }
	public UnityEvent OnDialogueStarted { get => _onDialogueStarted; }
	public UnityEvent<TextAsset> OnStartDialogue { get => _onStartDialogue; }
	#endregion



	#region MonoBehaviour Messages
	void Awake()
	{
		SetupFields();
	}
	#endregion



	private void SetupFields()
	{
		if (_onChooseDialogueOption == null) _onChooseDialogueOption = new UnityEvent<ChoiceButton>();
		if (_onShowNextText == null) _onShowNextText = new UnityEvent<string>();
		if (_onShowChoices == null) _onShowChoices = new UnityEvent();
		if (_onDialogueEnded == null) _onDialogueEnded = new UnityEvent();
		if (_onDialogueStarted == null) _onDialogueStarted = new UnityEvent();
		if(_onStartDialogue == null) _onStartDialogue = new UnityEvent<TextAsset>();
	}

	
}