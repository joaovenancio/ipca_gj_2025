using Ink.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DecipherRadioWave : MonoBehaviour
{
    [SerializeField] private List<PasswordTextAsset> _radioMessageSetup = new List<PasswordTextAsset>();


    private Dictionary<string, TextAsset> _radioMessages = new Dictionary<string, TextAsset>();

	[Header("Public Events")]
	[SerializeField] private UnityEvent _onDecipher;
	[SerializeField] private UnityEvent<string> _onEmitSound;


	private TextAsset _currentRadioMessage;

	public UnityEvent OnDecipher { get => _onDecipher; }
	public UnityEvent<string> OnEmitSound { get => _onEmitSound; }


    public float Timer =30f;


	private void Awake()
	{
		SetupDictionary();
		
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        if (_radioMessages == null) _radioMessages = new Dictionary<string, TextAsset>();
	}


	private void SetupDictionary()
	{
		foreach (PasswordTextAsset passwordTextAsset in _radioMessageSetup)
		{
			if (_radioMessages.ContainsKey(passwordTextAsset.Code)) continue;
			if (passwordTextAsset.Text == null) continue;

			_radioMessages.Add(passwordTextAsset.Code, passwordTextAsset.Text);
		}
	}


	void StartStory()
	{
		story = new Story(_currentRadioMessage.text);
		RefreshView();
	}

	public void Decipher(string code)
	{
		if (_radioMessages.ContainsKey(code))
		{
			_currentRadioMessage = _radioMessages[code];
			StartStory();
			_onDecipher.Invoke();
			_timeElapsed = 0f;
			IsTimerRunning = true;
		}
		else
		{
			Debug.LogWarning($"DecipherRadioWave: Code {code} not found in radio messages.");
		}
	}

	private float _timeElapsed = 0f;

	// Update is called once per frame
	void Update()
    {
		RadioTimer();
	}

	public bool IsTimerRunning = false;

	private void RadioTimer()
	{
		if (!IsTimerRunning) return;

		_timeElapsed += Time.deltaTime;

		if (_timeElapsed >= Timer)
		{
			RefreshView();
			_timeElapsed = 0f;
		}
	}

	[SerializeField]
	public Story story;

	void RefreshView()
	{
		if (!story.canContinue) return;
		
		string text = story.Continue().Trim();

		_onEmitSound.Invoke(text);
	}


}

[Serializable]
public struct PasswordTextAsset 
{
    public string Code;
    public TextAsset Text;

}
