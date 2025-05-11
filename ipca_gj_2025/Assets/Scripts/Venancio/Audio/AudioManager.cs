using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(-9)]
/// <summary>
/// Manages audio playback, including music, sound effects, and one-shot audio.
/// Provides methods to play, configure, and retrieve audio sources and information.
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
	#region Variables
	private Pool<OneShotAudio> _oneShotAudioPool;
	private Dictionary<string, AudioInformation> _musics;
	private Dictionary<string, AudioInformation> _effects;
	private Dictionary<string, AudioSource> _globalAudioSources;
	[Header("References")]
	[SerializeField] private AudioMixer _audioMixer;
	[SerializeField] private TagGlobalAudioSourcePair[] _globalAudioSourcesSetup;
	[SerializeField] private GameObject _oneshotPrefab;
	[Header("Settings")]
	[SerializeField] private TagAudioPair[] _musicDictionarySetup;
	[SerializeField] private TagAudioPair[] _effectsDictionarySetup;
	[Range(0f,1f)]
	[SerializeField] private float _masterVolume = 1f;
	[Range(0f, 1f)]
	[SerializeField] private float _musicVolume = 1f;
	[Range(0f, 1f)]
	[SerializeField] private float _effectsVolume = 1f;

	public float MasterVolume{
		get => _masterVolume;
		set	
		{
			_masterVolume = Mathf.Clamp(value, 0f, 1f);
			// TODO: Implement logic to update master volume in the audio mixer
		}
	}
	public float MusicVolume
	{
		get => _musicVolume;
		set
		{
			_musicVolume = Mathf.Clamp(value, 0f, 1f);
			// TODO: Implement logic to update music volume in the audio mixer
		}
	}
	public float EffectsVolume
	{
		get => _effectsVolume;
		set
		{
			_effectsVolume = Mathf.Clamp(value, 0f, 1f);
			// TODO: Implement logic to update effects volume in the audio mixer
		}
	}
	#endregion



	#region Unity Messages
	private void Awake()
	{
		_dontDestroyOnLoad = true;
		if (_oneShotAudioPool == null) _oneShotAudioPool = new Pool<OneShotAudio>(this.transform);
		SingletonSetup();
		SetupDictionaries();
		ClearReferences();
	}
	#endregion



	public AudioInformation GetMusicInformation(string musicName)
	{
		if (!_musics.ContainsKey(musicName))
		{
			Debug.Log("AudioManager -> GetMusic: No music with name " + musicName + " found.", this);
			return null;
		}

		return _musics[musicName];
	}

	/// <summary>
	/// Plays a music track on a specified global audio channel.
	/// </summary>
	/// <param name="globalChannel">The name of the global audio channel to play the music on.</param>
	/// <param name="musicName">The name of the music track to play.</param>
	public void PlayGlobal(string globalChannel, string musicName)
	{
		AudioSource globalAudioSource = GetGlobalSource(globalChannel);
		if (!globalAudioSource) return;

		AudioInformation musicToPlay = GetMusicInformation(musicName);
		if (musicToPlay == null) return;

		globalAudioSource.clip = musicToPlay.Clip;
		globalAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(musicToPlay.MixerGroup)[0];
		globalAudioSource.volume = musicToPlay.DefaultVolume;

		globalAudioSource.Play();
	}

	/// <summary>
	/// Retrieves a global audio source by its channel name.
	/// </summary>
	/// <param name="globalChannel">The name of the global audio channel.</param>
	/// <returns>The AudioSource associated with the specified global channel, or null if not found.</returns>
	public AudioSource GetGlobalSource(string globalChannel)
	{
		if (!_globalAudioSources.ContainsKey(globalChannel))
		{
			Debug.Log("AudioManager -> GetGlobalSource: No global audio source with name " + globalChannel + " found.", this);
			return null;
		}

		return _globalAudioSources[globalChannel];
	}

	/// <summary>
	/// Retrieves audio information for a specified audio effect.
	/// </summary>
	/// <param name="audioName">The name of the audio effect.</param>
	/// <returns>The AudioInformation associated with the specified audio effect, or null if not found.</returns>
	public AudioInformation GetAudioInformation(string audioName)
	{
		if (!_effects.ContainsKey(audioName))
		{
			Debug.Log("AudioManager -> GetAudioInformation: No audio with name " + audioName + " found.", this);
			return null;
		}

		return _effects[audioName];
	}

	/// <summary>
	/// Plays an audio clip on the specified target GameObject.
	/// </summary>
	/// <param name="audioName">The name of the audio clip to play.</param>
	/// <param name="target">The target GameObject on which to play the audio.</param>
	/// <returns>The AudioSource component used to play the audio, or null if the target is null or the there is no audio with given audioName.</returns>
	public AudioSource PlayAudio(string audioName, GameObject target) //TODO: test
	{
		if (!target)
		{
			Debug.LogWarning("AudioManager -> PlayAudio: Target is null", this);
			return null;
		}

		AudioSource audioSource = target.GetComponent<AudioSource>();

		if (!audioSource) audioSource = target.AddComponent<AudioSource>();

		if (!ConfigureAudioSource(new AudioSourcePreferences(audioName, audioSource)))
		{
			Debug.LogWarning("AudioManager -> PlayAudio: Audio source could not be configured", this);
			return null;
		}

		audioSource.Play();

		return audioSource;
	}

	/// <summary>
	/// Configures an AudioSource with the specified audio preferences.
	/// </summary>
	/// <param name="audioPreferences">The preferences for configuring the AudioSource, including the audio to play and the AudioSource component.</param>
	/// <returns>The configured AudioSource, or null if the configuration fails.</returns>
	public AudioSource ConfigureAudioSource(AudioSourcePreferences audioPreferences)
	{
		AudioInformation audioInformation = GetAudioInformation(audioPreferences.AudioToPlay);

		if (audioInformation == null)
		{
			Debug.LogWarning("AudioManager -> ConfigureAudioSource: No audio with tag " + audioPreferences.AudioToPlay + " found. Sender: " +
			audioPreferences.AudioSource.gameObject + ".", audioPreferences.AudioSource.gameObject);
			return null;
		}

		if (!audioPreferences.AudioSource)
		{
			Debug.LogWarning("AudioManager -> ConfigureAudioSource: AudioSource is null. Sender: " +
			audioPreferences.AudioSource.gameObject + ".", audioPreferences.AudioSource.gameObject);
			return null;
		}

		if (audioPreferences.IsUsingDefaultGroupVolume) audioPreferences.AudioSource.volume = audioInformation.DefaultVolume;
		audioPreferences.AudioSource.clip = audioInformation.Clip;
		audioPreferences.AudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(audioInformation.MixerGroup)[0];

		return audioPreferences.AudioSource;
	}

	/// <summary>
	/// Configures a one-shot audio object to play a specified audio clip at a given location.
	/// </summary>
	/// <param name="audioToPlay">The name of the audio clip to play.</param>
	/// <param name="location">The location where the audio should be played.</param>
	/// <returns>The configured OneShotAudio object, or null if the audio clip is not found.</returns>
	public OneShotAudio ConfigureOneShot(string audioToPlay, Vector3 location) //TODO: PlayOneShot()
	{
		AudioInformation audio = GetAudioInformation(audioToPlay); //TODO: implementar direto nessa classe, sem o ConfigureAudioSource

		if (audio == null) return null; //^

		OneShotAudio oneShot = _oneShotAudioPool.Retrieve();

		if (!oneShot) oneShot = CreateOneShotObject();

		//TODO: implement different prefabs for different audios
		//else
		if (!oneShot.AudioName.Equals(audioToPlay))
		{
			oneShot.AudioName = audioToPlay;
			ConfigureAudioSource(new AudioSourcePreferences(audioToPlay, oneShot.AudioSource));
		}

		oneShot.gameObject.transform.position = location;

		return oneShot;
	}

	public void FreeOneShot(OneShotAudio oneShotAudio)
	{
		_oneShotAudioPool.Free(oneShotAudio);
	}

	private OneShotAudio CreateOneShotObject()
	{
		GameObject oneShotGameObject = new GameObject("One shot Audio");
		oneShotGameObject.transform.SetParent(_oneShotAudioPool.ObjectsContainer);

		OneShotAudio oneShotAudio = oneShotGameObject.AddComponent<OneShotAudio>();
		AudioSource oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();

		oneShotAudio.AudioSource = oneShotAudioSource;
		oneShotAudio.AudioName = "";

		_oneShotAudioPool.Add(oneShotAudio);

		return oneShotAudio;
	}

	private void SetupDictionaries()
	{
		SetupGlobalAudioSources();
		SetupMusics();
		SetupEffects();
	}

	private void SetupMusics()
	{
		_musics = new Dictionary<string, AudioInformation>();

		if (_musicDictionarySetup == null) return;
		if (_musicDictionarySetup.Length == 0) return;

		foreach (TagAudioPair page in _musicDictionarySetup)
		{
			_musics.Add(page.Tag,
				new AudioInformation(page.Clip, page.MixerGroup, page.DefaultVolume) //Order
			);
		}
	}

	private void SetupGlobalAudioSources()
	{
		_globalAudioSources = new Dictionary<string, AudioSource>();

		if (_globalAudioSourcesSetup == null) return;
		if (_globalAudioSourcesSetup.Length == 0) return;

		foreach (TagGlobalAudioSourcePair page in _globalAudioSourcesSetup)
		{
			_globalAudioSources.Add(page.Tag, page.AudioSource);
		}
	}

	private void SetupEffects()
	{
		_effects = new Dictionary<string, AudioInformation>();

		if (_effectsDictionarySetup == null) return;
		if (_effectsDictionarySetup.Length == 0) return;

		foreach (TagAudioPair page in _effectsDictionarySetup)
		{
			_effects.Add(page.Tag,
				new AudioInformation(page.Clip, page.MixerGroup, page.DefaultVolume) //Order
			);
		}
	}

    private void ClearReferences()
    {
		_musicDictionarySetup = null;
		_effectsDictionarySetup = null;
		_globalAudioSourcesSetup = null;
	}
}

#region Structs
/// <summary>
/// Represents a pair of a tag and an audio clip, used for setting up a dictionary of audio clips.
/// </summary>
[Serializable]
public struct TagAudioPair
{
	/// <summary>
	/// The tag associated with the audio clip.
	/// </summary>
	public string Tag;
	/// <summary>
	/// The audio clip associated with the tag.
	/// </summary>
	public AudioClip Clip;
	/// <summary>
	/// The name of the mixer group to which this audio belongs.
	/// </summary>
	public string MixerGroup;
	/// <summary>
	/// The default volume level for this audio.
	/// </summary>
	[Range(0f, 1f)]
	public float DefaultVolume;



	/// <summary>
	/// Initializes a new instance of the <see cref="TagAudioPair"/> struct with the specified tag, clip, mixer group, and default volume.
	/// </summary>
	/// <param name="tag">The tag associated with the audio clip.</param>
	/// <param name="clip">The audio clip associated with the tag.</param>
	/// <param name="mixerGroup">The name of the mixer group to which this audio belongs.</param>
	/// <param name="defaultVolume">The default volume level for this audio.</param>
	public TagAudioPair(string tag, AudioClip clip, string mixerGroup, float defaultVolume)
	{
		Tag = tag;
		Clip = clip;
		MixerGroup = mixerGroup;
		DefaultVolume = defaultVolume;
	}
}

/// <summary>
/// Represents a pair of a tag and a global audio source, used for setting up a dictionary of global audio sources.
/// </summary>
[Serializable]
public struct TagGlobalAudioSourcePair
{
	/// <summary>
	/// The tag associated with the global audio source.
	/// </summary>
	public string Tag;
	/// <summary>
	/// The global audio source associated with the tag.
	/// </summary>
	public AudioSource AudioSource;
	/// <summary>
	/// Initializes a new instance of the <see cref="TagGlobalAudioSourcePair"/> struct with the specified tag and audio source.
	/// </summary>
	/// <param name="tag">The tag associated with the global audio source.</param>
	/// <param name="audioSource">The global audio source associated with the tag.</param>
	
	
	
	public TagGlobalAudioSourcePair(string tag, AudioSource audioSource)
	{
		Tag = tag;
		AudioSource = audioSource;
	}
}
#endregion