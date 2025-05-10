using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableItem : MonoBehaviour
{
    public ItemSO itemSO;
    public int amount;

    public Image icon;
    public TMP_Text nameText;

    float fontSize;

    private void Start()
    {
        SetItem();
    }

    public void SetItem()
    {
        fontSize = nameText.fontSize;
        icon.sprite = itemSO.icon;
        nameText.text = itemSO.name + " x" + amount;
    }

    public void DeleteItem()
    {
        Destroy(gameObject);
    }

    public void SetAmount(int amount)
    {
        this.amount = amount;
        nameText.text = itemSO.name + " x" + amount;
    }

    public void BtnHoverEnter()
    {
        StopAllCoroutines();
        StartCoroutine(OnBtnHoverEnter());
    }

    public void BtnHoverExit()
    {
        StopAllCoroutines();
        StartCoroutine(OnBtnHoverExit());
    }

    IEnumerator OnBtnHoverEnter()
    {
        yield return new WaitForSeconds(0.1f);
        nameText.fontSize = fontSize * 1.1f;
    }

    IEnumerator OnBtnHoverExit()
    {
        nameText.fontSize = fontSize;
        yield return new WaitForSeconds(0.1f);
    }
    
    public void BtnClick()
    {
        Debug.Log("Clicked " + itemSO.name);
        StopAllCoroutines();
        LevelManager.instance.SelectItem(itemSO);
    }
}
