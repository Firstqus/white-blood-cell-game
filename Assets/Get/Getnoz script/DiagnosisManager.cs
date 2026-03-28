using UnityEngine;
using Unity.Cinemachine; // <--- สำคัญมากสำหรับ Unity 6
using System.Collections;

public class DiagnosisManager : MonoBehaviour
{
    // ช่องสำหรับลาก Cinemachine Camera มาใส่
    public CinemachineCamera vCam; 
    
    // ช่องสำหรับลาก Panel UI ที่เก็บปุ่มมาใส่
    public GameObject uiPanel; 

    public float targetFOV = 20f;
    public float zoomDuration = 2f;

    // ฟังก์ชันนี้ไว้เชื่อมกับปุ่ม (On Click)
    public void SelectAnswer(string type)
    {
        // ปิด UI เมื่อเลือกแล้ว
        if (uiPanel != null) uiPanel.SetActive(false);
        
        BGMPlayer.Instance?.FadeOut(zoomDuration);
        
        // เริ่มซูม
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
    }
}