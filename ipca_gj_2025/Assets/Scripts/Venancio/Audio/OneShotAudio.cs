using UnityEngine;

public class OneShotAudio : MonoBehaviour, IFactoryObject<OneShotAudio>, IPoolable
{
	#region Fields
	public string AudioName;
	public AudioSource AudioSource;
	private AudioManager _audioManager;
	private bool _isPlaying = false;

	#endregion

	#region MonoBehaviour Messages
	void Awake()
	{
		SetupFields();
	}

	void Update()
	{
		DespawnWhenFinishedPlaying();
	}
	#endregion

	private void SetupFields()
	{
		_audioManager = AudioManager.Instance;
		if (!AudioSource) AudioSource = this.GetComponent<AudioSource>();
	}

	private void DespawnWhenFinishedPlaying()
	{
		if (!_isPlaying) return;

		if (AudioSource.isPlaying) return;

		_isPlaying = false;
		_audioManager.FreeOneShot(this);
	}

	public void Play()
	{
		AudioSource.Play();
		_isPlaying = true;
	}

	#region Interfaces
	public GameObject MyInstance => this.gameObject;

	public void OnFree()
	{
		AudioSource.Stop();
	}

	public void OnRepurpose(OneShotAudio newObject)
	{
		if (AudioName.Equals(newObject.AudioName)) return;

		this.AudioSource.clip = newObject.AudioSource.clip;
		this.AudioSource.volume = newObject.AudioSource.volume;
		this.AudioSource.pitch = newObject.AudioSource.pitch;
		this.AudioSource.loop = newObject.AudioSource.loop;
		this.AudioSource.mute = newObject.AudioSource.mute;
		this.AudioSource.bypassEffects = newObject.AudioSource.bypassEffects;
		this.AudioSource.bypassListenerEffects = newObject.AudioSource.bypassListenerEffects;
		this.AudioSource.bypassReverbZones = newObject.AudioSource.bypassReverbZones;
		this.AudioSource.playOnAwake = newObject.AudioSource.playOnAwake;
		this.AudioSource.panStereo = newObject.AudioSource.panStereo;
		this.AudioSource.spatialBlend = newObject.AudioSource.spatialBlend;
		this.AudioSource.reverbZoneMix = newObject.AudioSource.reverbZoneMix;
		this.AudioSource.dopplerLevel = newObject.AudioSource.dopplerLevel;
		this.AudioSource.spread = newObject.AudioSource.spread;
		this.AudioSource.priority = newObject.AudioSource.priority;
		this.AudioSource.minDistance = newObject.AudioSource.minDistance;
		this.AudioSource.maxDistance = newObject.AudioSource.maxDistance;
		this.AudioSource.rolloffMode = newObject.AudioSource.rolloffMode;
	}

	public void OnRetrieve()
	{
		
	}
	#endregion
}
