using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer mainMixer;
    public Slider     sliderVolume;
    public TextMeshProUGUI textVolume;
    public Toggle     toggleMute;

    float lastVolume = 0.8f;

    void Start()
    {
        float saved = PlayerPrefs.GetFloat("Volume", 0.8f);
        sliderVolume.value = saved;
        ApplyVolume(saved);

        toggleMute.isOn = PlayerPrefs.GetInt("Mute", 0) == 1;
    }

    public void OnSliderChanged(float value)
    {
        lastVolume = value;
        ApplyVolume(value);
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void OnVolUp()
    {
        sliderVolume.value = Mathf.Clamp(sliderVolume.value + 0.1f, 0f, 1f);
    }

    public void OnVolDown()
    {
        sliderVolume.value = Mathf.Clamp(sliderVolume.value - 0.1f, 0f, 1f);
    }

    public void OnMuteToggle(bool isMuted)
    {
        ApplyVolume(isMuted ? 0f : lastVolume);
        if (!isMuted) sliderVolume.value = lastVolume;
        PlayerPrefs.SetInt("Mute", isMuted ? 1 : 0);
    }

    public void OnBackPressed()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }

    void ApplyVolume(float linear)
    {
        float dB = linear > 0.0001f ? Mathf.Log10(linear) * 20f : -80f;
        Debug.Log($"Set volume: linear={linear}, dB={dB}");  // ← เพิ่มบรรทัดนี้
        bool success = mainMixer.SetFloat("MasterVolume", dB);
        Debug.Log($"SetFloat success: {success}");  // ← และบรรทัดนี้
        textVolume.text = Mathf.RoundToInt(linear * 100) + "%";
    }
}