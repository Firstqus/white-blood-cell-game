using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer Instance;
    public AudioMixer mainMixer;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator Start()
    {
        // รอให้ AudioMixer พร้อมก่อน 1 frame
        yield return new WaitForEndOfFrame();

        float saved = PlayerPrefs.GetFloat("Volume", 0.8f);
        float dB = saved > 0.0001f ? Mathf.Log10(saved) * 20f : -80f;
        mainMixer.SetFloat("MasterVolume", dB);
    }
}