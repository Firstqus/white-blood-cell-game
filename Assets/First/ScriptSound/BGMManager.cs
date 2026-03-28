using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("BGM")]
    public AudioClip bgmClip;
    [Range(0f, 1f)]
    public float volume = 0.5f;

    AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ← เพลงไม่ขาดตอนเปลี่ยน scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = bgmClip;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        if (bgmClip != null) audioSource.Play();
    }

    public void SetVolume(float v) => audioSource.volume = v;
    public void Stop() => audioSource.Stop();
    public void Play() => audioSource.Play();

    [Header("Game Over")]
    public AudioClip gameOverClip;

    public void PlayGameOver()
    {
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = gameOverClip;
        audioSource.Play();
    }
    [Header("Victory")]
    public AudioClip victoryClip;

    public void PlayVictory()
    {
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = victoryClip;
        audioSource.Play();
    }
    public void PlayBGM()
    {
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = bgmClip;
        audioSource.Play();
    }
}