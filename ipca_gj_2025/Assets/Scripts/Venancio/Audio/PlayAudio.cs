using UnityEngine;

public class PlayAudio : MonoBehaviour
{
	#region Fields
	private AudioManager _audioManager;
    [Header("Settings")]
	[SerializeField] private string _audioName;
	[SerializeField] private bool _isUsingDefaultGroupVolume = false;
	[Range(0f,1f)][SerializeField] private float _volume = 1f;
	[SerializeField] private bool _isPlayingOnAwake = false;
	[Header("Optional Reference")]
	[SerializeField] private AudioSource _audioSource;

	public string AudioName	{ get => _audioName; set => _audioName = value;	}
	public AudioSource AudioSource { get => _audioSource; set => _audioSource = value; }
	public bool IsPlayingOnAwake { get => _isPlayingOnAwake;	set => _isPlayingOnAwake = value; }
	#endregion



	#region Unity Messages
	private void Awake()
	{
		SetupFields();
	}

	void Start()
    {
		if (_isPlayingOnAwake) _audioSource.Play();
	}
	#endregion



	private void SetupFields()
	{
		_audioManager = AudioManager.Instance;
		CreateAudioSource();
		_audioManager.ConfigureAudioSource(new AudioSourcePreferences(_audioName, _audioSource, _isUsingDefaultGroupVolume));
	}

	public void CreateAudioSource()
	{
		if (AudioSource) return;

		AudioSource = this.gameObject.AddComponent<AudioSource>();
		AudioSource.volume = _volume;
		AudioSource.playOnAwake = false;
	}

	public void Play()
	{
		_audioSource.Play();
	}
}
