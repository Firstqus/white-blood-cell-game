using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class DiagnosisManager : MonoBehaviour
{
    public CinemachineCamera vCam; 
    public GameObject uiPanel; 

    public float targetFOV = 20f;
    public float zoomDuration = 2f;
    public float delayBeforeLoad = 4f;

    [Header("Wrong Answer")]
    public TextMeshProUGUI wrongText;

    [Header("Sound")]
    public AudioClip clickSound;
    public AudioClip wrongSound;  // เสียงตอบผิด
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

    public void SelectAnswer(string type)
    {
        if (type != "Bacteria")
        {
            if (wrongSound != null)
                audioSource.PlayOneShot(wrongSound);  // เสียงผิด

            int wrongCount = PlayerPrefs.GetInt("WrongCount", 0);
            PlayerPrefs.SetInt("WrongCount", wrongCount + 1);
            
            StartCoroutine(ShowWrongMessage());
            return;
        }

        PlayClick();  // เสียงถูก

        if (uiPanel != null) uiPanel.SetActive(false);
        BGMPlayer.Instance?.FadeOut(zoomDuration);
        StartCoroutine(ExecuteZoom());
    }

    IEnumerator ShowWrongMessage()
    {
        if (wrongText == null) yield break;

        wrongText.text = "ไม่ใช่! แผลข่วนธรรมดาเกิดจาก Bacteria นะ";
        wrongText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        wrongText.gameObject.SetActive(false);
    }

    IEnumerator ExecuteZoom()
    {
        if (vCam == null) yield break;

        float startFOV = vCam.Lens.FieldOfView;
        float elapsed = 0f;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            vCam.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsed / zoomDuration);
            yield return null;
        }

        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}