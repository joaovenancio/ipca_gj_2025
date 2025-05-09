using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("Referencess")]
    [SerializeField] private GameObject _dialoguePanel;
	[SerializeField] private TMPro.TMP_Text _dialogueText;
    [SerializeField] private TMPro.TMP_Text _speakerName;
    [SerializeField] private GameObject _choicesPanel;
	[SerializeField] private List<ChoiceButton> _choiceButtons;


	public GameObject DialoguePanel => _dialoguePanel;
	public TMP_Text DialogueText => _dialogueText;
	public TMP_Text SpeakerName => _speakerName;
	public List<ChoiceButton> OptionsButtons => _choiceButtons;
	public GameObject OptionsPanel => _choicesPanel;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
