using UnityEngine;
using UnityEngine.Events;

public class DialogueEvents : MonoBehaviour
{
	#region Fields
	[Header("Public Events")]
	[Tooltip("Event triggered when a dialogue option is chosen. Parameters: ChoiceButton and option index.")]
	[Space, SerializeField] private UnityEvent<ChoiceButton> _onChooseDialogueOption;
	[Space, SerializeField] private UnityEvent<string> _onShowNextText;
	[Space, SerializeField] private UnityEvent<string> _onShowChoices;
	[Space, SerializeField] private UnityEvent<string> _onHideChoices;



	public UnityEvent<ChoiceButton> OnChooseDialogueOption { get => _onChooseDialogueOption; }
	public UnityEvent<string> OnShowNextText { get => _onShowNextText; }
	public UnityEvent<string> OnShowChoices { get => _onShowChoices; }
	public UnityEvent<string> OnHideChoices { get => _onHideChoices; }
	#endregion



	#region MonoBehaviour Messages
	void Awake()
	{
		SetupFields();
	}

	void Update()
	{

	}
	#endregion


	private void SetupFields()
	{
		if (_onChooseDialogueOption == null) _onChooseDialogueOption = new UnityEvent<ChoiceButton>();
		if (_onShowNextText == null) _onShowNextText = new UnityEvent<string>();
		if (_onShowChoices == null) _onShowChoices = new UnityEvent<string>();
		if (_onHideChoices == null) _onHideChoices = new UnityEvent<string>();
	}

	
}