using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DiagnosisManager : MonoBehaviour
{
    public CinemachineCamera vCam; 
    public GameObject uiPanel; 

    public float targetFOV = 20f;
    public float zoomDuration = 2f;
    public float delayBeforeLoad = 4f; // ← รอกี่วิก่อนวาป

    public void SelectAnswer(string type)
    {
        if (uiPanel != null) uiPanel.SetActive(false);
        
        BGMPlayer.Instance?.FadeOut(zoomDuration);
        
        StartCoroutine(ExecuteZoom());
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

        // ซูมจบแล้ว รอ 4 วิ แล้ววาปไป Scene ถัดไป
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}