using System;
using UnityEngine;

/// <summary>
/// Represents audio information including the audio clip, mixer group, and default volume.
/// </summary>
[Serializable]
public class AudioInformation
{
	/// <summary>
	/// The audio clip to be played.
	/// </summary>
	public AudioClip Clip;
	/// <summary>
	/// The name of the mixer group to which this audio belongs.
	/// </summary>
	public string MixerGroup;
	/// <summary>
	/// The default volume level for this audio.
	/// </summary>
	public float DefaultVolume;



	/// <summary>
	/// Initializes a new instance of the <see cref="AudioInformation"/> class with the specified clip, mixer group, and default volume.
	/// </summary>
	/// <param name="clip">The audio clip to be played.</param>
	/// <param name="mixerGroup">The name of the mixer group to which this audio belongs.</param>
	/// <param name="defaultVolume">The default volume level for this audio.</param>
	public AudioInformation(AudioClip clip, string mixerGroup, float defaultVolume)
	{
		Clip = clip;
		DefaultVolume = defaultVolume;
		MixerGroup = mixerGroup;
	}
}