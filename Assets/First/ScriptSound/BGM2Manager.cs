using UnityEngine;

public class BGM2Manager : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] [Range(0f, 1f)] private float volume = 1f;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = bgmClip;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        Play();
    }

    public void Play()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void SetVolume(float v)
    {
        audioSource.volume = Mathf.Clamp01(v);
    }
}