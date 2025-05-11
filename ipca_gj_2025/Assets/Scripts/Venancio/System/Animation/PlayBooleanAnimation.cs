//Copyright(C) 2025 Joao Vitor Demaria Venancio under GNU AGPL. Refer to README.md for more information.
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayBooleanAnimation : MonoBehaviour
{
    public bool isPlaying = false;
    public float hoverSpeed = 2f;
    public float hoverHeight = 10f;

    private RectTransform rectTransform;
    private Vector2 initialAnchoredPos;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialAnchoredPos = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        if (isPlaying)
        {
            float yOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
            rectTransform.anchoredPosition = initialAnchoredPos + new Vector2(0f, yOffset);
        }
        else
        {
            rectTransform.anchoredPosition = initialAnchoredPos;
        }
    }

    public void Play(bool state)
    {
        isPlaying = state;
    }
}