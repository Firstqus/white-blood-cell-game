using UnityEngine;
using TMPro; 
using Unity.Cinemachine; 
using System.Collections;
using UnityEngine.SceneManagement; // เพิ่มเพื่อใช้ในการเปลี่ยน Scene
using UnityEngine.UI; // เพิ่มเพื่อควบคุม UI Button

public class EndingManager : MonoBehaviour
{
    public CinemachineCamera vCam;
    public TextMeshProUGUI endText;
    public GameObject blackOverlay;
    
    // --- เพิ่มส่วนนี้ ---
    [Header("UI Navigation")]
    public GameObject backToMenuButton; // ลากปุ่มจาก Inspector มาใส่ช่องนี้
    // ------------------

    [Header("Settings")]
    public float startFOV = 20f;
    public float endFOV = 60f;
    public float zoomOutDuration = 4f;
    
    [TextArea(5, 10)]
    public string fullContent; 

    void Start()
    {
        blackOverlay.SetActive(false);
        
        // --- เพิ่มส่วนนี้: ปิดปุ่มไว้ก่อนตอนเริ่ม ---
        if (backToMenuButton != null) 
            backToMenuButton.SetActive(false);
        // ------------------

        endText.text = "";
        vCam.Lens.FieldOfView = startFOV;
        
        StartCoroutine(PlayEndingSequence());
    }

    IEnumerator PlayEndingSequence()
    {
        float elapsed = 0f;

        // 1. ค่อยๆ ซูมออก
        while (elapsed < zoomOutDuration)
        {
            elapsed += Time.deltaTime;
            vCam.Lens.FieldOfView = Mathf.Lerp(startFOV, endFOV, elapsed / zoomOutDuration);
            yield return null;
        }

        // 2. แสดงพื้นหลังดำจางๆ
        blackOverlay.SetActive(true);

        // 3. ค่อยๆ พิมพ์ตัวอักษร (Typewriter Effect)
        foreach (char c in fullContent)
        {
            endText.text += c;
            yield return new WaitForSeconds(0.05f); 
        }

        // --- เพิ่มส่วนนี้: แสดงปุ่มหลังจากพิมพ์จบ ---
        yield return new WaitForSeconds(0.5f); // รอจังหวะนิดนึงให้ดูนุ่มนวล
        if (backToMenuButton != null)
        {
            backToMenuButton.SetActive(true);
        }
        // ------------------
    }

    // --- เพิ่มฟังก์ชันสำหรับให้ปุ่มเรียกใช้ ---
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // เปลี่ยนชื่อ "MainMenu" ให้ตรงกับชื่อ Scene ของคุณ
    }
}