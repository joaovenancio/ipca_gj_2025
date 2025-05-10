using UnityEngine;

public class ChangeText : MonoBehaviour
{
	[Header("References")]
    [SerializeField] private TMPro.TMP_Text _textToChange;



	private void Awake()
	{
		SetupFields();
	}



	private void SetupFields()
	{
		if (!_textToChange) _textToChange = this.GetComponent<TMPro.TMP_Text>();
		if (!_textToChange) _textToChange = this.GetComponentInChildren<TMPro.TMP_Text>();
		if (!_textToChange)
		{
			Debug.LogError("ChangeText: No TMP_Text component found.", this);
			this.enabled = false;
			return;
		}
	}

	public void Change(string newText)
    {
        if (!_textToChange) return;

		_textToChange.text = newText;
	}
}
