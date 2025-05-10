using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MenuController : MonoBehaviour
{

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;
    
    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    private float initialVolume = 0.0f;

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void ExitGame()
    {
        Debug.Log("Exit game success");
        Application.Quit();
    }



    //*****************
    //--- Volume ---
    //*****************
    public void getVolume()
    {
        initialVolume = AudioListener.volume;
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeBack(string back)
    {
        if (back == "back")
        {
            AudioListener.volume = initialVolume;
            volumeSlider.value = initialVolume;
            volumeTextValue.text = initialVolume.ToString("0.0");
        }
    }

    public void VolumeApply() 
    {
        initialVolume = AudioListener.volume;
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
        
        //Debug.Log("volume: " + AudioListener.volume);
    }

    public void ResetVolume(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
