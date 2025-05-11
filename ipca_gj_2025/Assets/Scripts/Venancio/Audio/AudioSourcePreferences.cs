using System;
using UnityEngine;

/// <summary>
/// Represents preferences for configuring an AudioSource with a specific audio clip.
/// </summary>
[Serializable]
public struct AudioSourcePreferences
{
	/// <summary>
	/// The name of the audio clip to play.
	/// </summary>
	public string AudioToPlay;
	/// <summary>
	/// The AudioSource component to configure.
	/// </summary>
	public AudioSource AudioSource;
	/// <summary>
	/// Indicates whether to use the default volume level for the audio group.
	/// </summary>
	public bool IsUsingDefaultGroupVolume;

	

	/// <summary>
	/// Initializes a new instance of the <see cref="AudioSourcePreferences"/> struct with the specified audio clip and AudioSource.
	/// </summary>
	/// <param name="audioToPlay">The name of the audio clip to play.</param>
	/// <param name="audioSource">The AudioSource component to configure.</param>
	public AudioSourcePreferences(string audioToPlay, AudioSource audioSource)
	{
		this.AudioToPlay = audioToPlay;
		this.AudioSource = audioSource;
		this.IsUsingDefaultGroupVolume = true;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="AudioSourcePreferences"/> struct with the specified audio clip, AudioSource, and volume preference.
	/// </summary>
	/// <param name="audioToPlay">The name of the audio clip to play.</param>
	/// <param name="audioSource">The AudioSource component to configure.</param>
	/// <param name="isUsingDefaultGroupVolume">Indicates whether to use the default volume level for the audio group.</param>
	public AudioSourcePreferences(string audioToPlay, AudioSource audioSource, bool isUsingDefaultGroupVolume)
	{
		this.AudioToPlay = audioToPlay;
		this.AudioSource = audioSource;
		this.IsUsingDefaultGroupVolume = isUsingDefaultGroupVolume;
	}
}
