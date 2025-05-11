using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GamePartManager : MonoBehaviour
{
    public static GamePartManager instance;

    public GameObject shipArea;      // The inventory part (always active)
    public GameObject canteenUI;     // Dialogue UI (enable/disable)
    public GameObject radioUI;       // Radio UI (enable/disable)

    public Button leftButton;        // Arrow to go left (to canteen)
    public Button rightButton;       // Arrow to go right (to radio)

    private enum Part { Canteen, Ship, Radio }
    private Part currentPart = Part.Ship;

    public Transform shipCamPos;
    public Transform canteenCamPos;
    public Transform radioCamPos;
    public float cameraMoveSpeed = 5f;
    private Transform mainCam;

    private void Awake()
    {
        instance = this;
        mainCam = Camera.main.transform;

        canteenUI.SetActive(false);
        radioUI.SetActive(false);

        UpdateUI();
    }

    private void Update()
    {
        Vector3 target = GetTargetCamPosition();
        mainCam.position = Vector3.Lerp(mainCam.position, target, Time.deltaTime * cameraMoveSpeed);
    }

    private Vector3 GetTargetCamPosition()
    {
        return currentPart switch
        {
            Part.Canteen => canteenCamPos.position,
            Part.Ship => shipCamPos.position,
            Part.Radio => radioCamPos.position,
            _ => shipCamPos.position
        };
    }

    public void OnLeftButton()
    {
        if (currentPart == Part.Ship) currentPart = Part.Canteen;
        else if (currentPart == Part.Radio) currentPart = Part.Ship;
        UpdateUI();
    }

    public void OnRightButton()
    {
        if (currentPart == Part.Ship) currentPart = Part.Radio;
        else if (currentPart == Part.Canteen) currentPart = Part.Ship;
        UpdateUI();
    }

    private void UpdateUI()
    {
        StopAllCoroutines(); // cancel previous transitions
        StartCoroutine(HandleUIVisibility());

        leftButton.interactable = currentPart != Part.Canteen;
        rightButton.interactable = currentPart != Part.Radio;
    }

    private IEnumerator HandleUIVisibility()
    {
        canteenUI.SetActive(false);
        radioUI.SetActive(false);

        yield return new WaitForSeconds(0.4f);

        canteenUI.SetActive(currentPart == Part.Canteen);
        radioUI.SetActive(currentPart == Part.Radio);
    }
}