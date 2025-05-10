//Copyright(C) 2025 Joao Vitor Demaria Venancio under GNU AGPL. Refer to README.md for more information.
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayBooleanAnimation : MonoBehaviour
{
	[Header("Reference")]
	public Animator _animator;
	[Header("Settings")]
	public string _parameterName;



	public void Play(bool state)
	{
		_animator.SetBool(_parameterName, state);
	}
}
