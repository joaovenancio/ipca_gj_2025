using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;
using UnityEngine.Events;


public class InkManager : MonoBehaviour //TO-DO: Separete the choices logic from the dialogue logic
{
    [SerializeField] private DialogueUI _dialogueUI;
	[SerializeField] private TextAsset inkJSONAsset = null;
	[SerializeField] private List<DialogueFunction> _inkFunctionsSetup;


	private Dictionary<string, UnityEvent<string>> _inkFunctions;


	public Story story;
	private int _currentChoiceIndex = 0;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		StartStory();

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void StartStory()
	{
		story = new Story(inkJSONAsset.text);
		//if (OnCreateStory != null) OnCreateStory(story);
		RefreshView();
		//Refresh variables
	}

	private void SetupDialogueText(string text)
	{
		_dialogueUI.DialogueText.text = text;
	}

	private void ShowDialogueUI ()
	{
		_dialogueUI.DialoguePanel.SetActive(true);
		_dialogueUI.OptionsPanel.SetActive(false);
	}


	public void RefreshView()
	{

		if (story.canContinue)
		{
			ShowDialogueUI();

			string text = story.Continue();
			text = text.Trim();

			SetupDialogueText(text);

		} else if (story.currentChoices.Count > 0)
		{
			SetupChoices();
		}
		else
		{
			EndStory();
		}

		// Display all the choices, if there are any!
		//if (story.currentChoices.Count > 0)
		//{
		//	for (int i = 0; i < story.currentChoices.Count; i++)
		//	{
		//		Choice choice = story.currentChoices[i];
		//		Button button = CreateChoiceView(choice.text.Trim());
		//		// Tell the button what to do when we press it
		//		button.onClick.AddListener(delegate {
		//			OnClickChoiceButton(choice);
		//		});
		//	}
		//}
		//// If we've read all the content and there's no choices, the story is finished!
		//else
		//{
		//	Button choice = CreateChoiceView("End of story.\nRestart?");
		//	choice.onClick.AddListener(delegate {
		//		StartStory();
		//	});
		//}
	}

	private void SetupChoices()
	{
		if (story.currentChoices.Count > _dialogueUI.OptionsButtons.Count)
		{
			Debug.LogError("InkManager: Not enough buttons to display all choices.", this);
			return;
		}

		for (int i = 0; i < story.currentChoices.Count; i++)
		{
			Choice choice = story.currentChoices[i];
			ChoiceButton button = _dialogueUI.OptionsButtons[i];

			SetupButtonContent(button, choice);

			button.gameObject.SetActive(true);

		}


	}

	private void SetupButtonContent(ChoiceButton button, Choice choice)
	{
		button.ButtonText.text = choice.text.Trim();
		button.Option = choice.index;
		button.ChoiceIndex = choice.index;
		button.gameObject.SetActive(true);
	}

	private void DisableChoices()
	{

	}

	private void EndStory()
	{
		Debug.LogError("END OF STORY");
	}

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton(Choice choice)
	{
		story.ChooseChoiceIndex(choice.index);
		RefreshView();
	}
}

public struct DialogueFunction
{
	public string FunctionName;
	public UnityEvent<string> FunctionEvent;
}
