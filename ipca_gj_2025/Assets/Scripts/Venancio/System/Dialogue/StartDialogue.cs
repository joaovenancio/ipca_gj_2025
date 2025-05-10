using UnityEngine;

public class StartDialogue : MonoBehaviour
{
	[SerializeField] private TextAsset _inkJSONAsset = null;
    [SerializeField] private DialogueEvents _dialogueEvents;

    [SerializeField] private bool _isPlayingOnStart = false;

	private void Awake()
	{
        SetupFields();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        if (_isPlayingOnStart) Play();
    }

    // Update is called once per frame
    void Update()
    {
        
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
