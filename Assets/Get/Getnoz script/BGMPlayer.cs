using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer Instance;
    public AudioMixer mainMixer;
    public AudioSource audioSource;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        float saved = PlayerPrefs.GetFloat("Volume", 0.8f);
        float dB = saved > 0.0001f ? Mathf.Log10(saved) * 20f : -80f;
        mainMixer.SetFloat("MasterVolume", dB);
    }

    public void FadeOut(float duration = 1.5f)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}