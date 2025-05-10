using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Radio : MonoBehaviour
{

    [SerializeField] private TMP_Text textToInsert;

    [Space, SerializeField] private UnityEvent<string> onSendEvents;
    public UnityEvent<string> OnSendEvents { get => onSendEvents; }

    public void GetValue(string numero)
    {
        textToInsert.text = textToInsert.text + numero;
    }

    public void DeleteTexto()
    {
        textToInsert.text = "";
    }

    public void ClickSendCodigo() 
    {
        onSendEvents.Invoke(textToInsert.text);
    }
}
