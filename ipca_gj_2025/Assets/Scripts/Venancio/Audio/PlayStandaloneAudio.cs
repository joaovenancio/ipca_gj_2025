using System;
using UnityEngine;

public class PlayStandaloneAudio : MonoBehaviour
{
	#region Fields
	private AudioManager _audioManager;
	[Header("Settings")]
	[SerializeField] private string _audioName;
	[SerializeField] private bool _isUsingDefaultGroupVolume = false;
	[Range(0f, 1f)][SerializeField] private float _volume = 1f;
	[SerializeField] private bool _isPlayingOnAwake = false;
	[SerializeField] private Transform _locationToPlay;

	public string AudioName { get => _audioName; set => _audioName = value; }
	public bool IsPlayingOnAwake { get => _isPlayingOnAwake; set => _isPlayingOnAwake = value; }
	public float Volume { get => _volume; set => _volume = value; }
	public Transform LocationToPlay { get => _locationToPlay; set => _locationToPlay = value; }
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
		_audioManager = AudioManager.Instance;
		if (!_locationToPlay) _locationToPlay = this.transform;
	}

	public void Play()
	{
		_audioManager.ConfigureOneShot(_audioName, _locationToPlay.position).Play();
	}
}
