using UnityEngine;

public class StartDialogue : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private TextAsset _inkJSONAsset = null;
	[Header("Optional References")]
    [SerializeField] private DialogueEvents _dialogueEvents;
	[Header("Settings")]
	[SerializeField] private bool _isPlayingOnStart = false;

    [Header("Variables")]
    public bool IsAlreadyPlayed = false;


	private void OnEnable()
	{
		_dialogueEvents.OnStartDialogue.AddListener(CheckForDialgueAlreadyPlayed);
	}

	private void OnDisable()
	{
		_dialogueEvents.OnStartDialogue.RemoveListener(CheckForDialgueAlreadyPlayed);
	}



	private void Awake()
	{
        SetupFields();
	}

	void Start()
    {
        if (_isPlayingOnStart) Play();
    }



    public void Play(TextAsset textAsset)
    {
        _inkJSONAsset = textAsset;
        
        Play();
	}

    public void Play()
    {
        if (!_inkJSONAsset) return;


		_dialogueEvents.OnStartDialogue.Invoke(_inkJSONAsset);
	}

    private void CheckForDialgueAlreadyPlayed(TextAsset textAsset)
    {
        if (textAsset.Equals(_inkJSONAsset)) IsAlreadyPlayed = true;
	}

    private void SetupFields()
    {
        if (!_inkJSONAsset)
        {
            Debug.LogWarning("StartDialogue: Ink JSON Asset is not assigned in the inspector.");
            this.enabled = false;
			return;
		}

        if (!_dialogueEvents) _dialogueEvents = EventManager2D.Instance.DialogueEvents;

	}
}
