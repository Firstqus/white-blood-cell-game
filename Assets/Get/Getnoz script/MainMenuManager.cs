using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Sound")]
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void PlayClick()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }

    public void OnStartGame()
    {
        PlayClick();
        SceneManager.LoadScene("Tutorial");
    }

    public void OnSettingsPressed()
    {
        PlayClick();
        SceneManager.LoadScene("Settings");
    }

    public void QuitGame()
    {
        PlayClick();
        PlayerPrefs.Save();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}