using UnityEngine;
using TMPro; 
using Unity.Cinemachine; 
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    public CinemachineCamera vCam;
    public TextMeshProUGUI endText;
    public GameObject blackOverlay;
    
    [Header("UI Navigation")]
    public GameObject backToMenuButton;

    [Header("Settings")]
    public float startFOV = 20f;
    public float endFOV = 60f;
    public float zoomOutDuration = 4f;
    
    [Header("Sound")]
    public AudioClip typingSound;
    public AudioClip bgmClip;
    private AudioSource audioSource;
    private AudioSource bgmSource;

    [TextArea(5, 10)]
    public string fullContent; 

    void Start()
    {
        // AudioSource สำหรับเสียงพิมพ์
        audioSource = gameObject.AddComponent<AudioSource>();

        // AudioSource สำหรับ BGM
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();

        blackOverlay.SetActive(false);
        
        if (backToMenuButton != null) 
            backToMenuButton.SetActive(false);

        endText.text = "";
        vCam.Lens.FieldOfView = startFOV;
        
        StartCoroutine(PlayEndingSequence());
    }

    IEnumerator PlayEndingSequence()
    {
        float elapsed = 0f;

        while (elapsed < zoomOutDuration)
        {
            elapsed += Time.deltaTime;
            vCam.Lens.FieldOfView = Mathf.Lerp(startFOV, endFOV, elapsed / zoomOutDuration);
            yield return null;
        }

        blackOverlay.SetActive(true);

        foreach (char c in fullContent)
        {
            endText.text += c;

            if (typingSound != null && c != ' ' && c != '\n' && !audioSource.isPlaying)
                audioSource.PlayOneShot(typingSound);

            yield return new WaitForSeconds(0.05f); 
        }

        audioSource.Stop();

        yield return new WaitForSeconds(0.5f);
        if (backToMenuButton != null)
            backToMenuButton.SetActive(true);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}