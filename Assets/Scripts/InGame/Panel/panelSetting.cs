using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class panelSetting : MonoBehaviour
{
    public TextMeshProUGUI BGMText;
    public TextMeshProUGUI SFXText;
    public TextMeshProUGUI BTNText;
    public Slider BGMSlider;
    public Slider SFXSlider;
    public Slider BTNSlider;

    // Start is called before the first frame update
    public void OnEnable()
    {
        BGMSlider.value = PlayerPrefs.GetFloat("BGM");
        BGMText.text = ((int)(BGMSlider.value * 10)).ToString();
        SFXSlider.value = PlayerPrefs.GetFloat("SFX");
        SFXText.text = ((int)(SFXSlider.value * 10)).ToString();
        BTNSlider.value = PlayerPrefs.GetFloat("BTN");
        BTNText.text = ((int)(BTNSlider.value * 10)).ToString();
    }
    public void onValueChangeBGM(float value)
    {
        PlayerPrefs.SetFloat("BGM", value);
        SoundManager.instance.masterVolumeBGM = value;
        BGMText.text = ((int)(BGMSlider.value * 10)).ToString();
    }
    public void onValueChangeSFX(float value)
    {
        PlayerPrefs.SetFloat("SFX", value);
        SoundManager.instance.masterVolumeSFX = value;
        SFXText.text = ((int)(SFXSlider.value * 10)).ToString();
    }
    public void onValueChangeBTN(float value)
    {
        PlayerPrefs.SetFloat("BTN", value);
        SoundManager.instance.masterVolumeBTN = value;
        BTNText.text = ((int)(BTNSlider.value * 10)).ToString();
    }
    public void onClickDELETE()
    {
        SoundManager.instance.PlayBTNSound("Menu_Select_00");
        PlayerPrefs.DeleteAll();
        DataManager.instance.DeleteAllData();
    }
    public void onClickQuit()
    {
        SoundManager.instance.PlayBTNSound("Menu_Select_00");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
