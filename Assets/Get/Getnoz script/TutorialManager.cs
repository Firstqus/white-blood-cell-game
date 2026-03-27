using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("Card UI")]
    public Image       accentBar;       // แถบสีด้านบน card
    public Image       iconCircle;      // วงกลม icon
    public Image mainIcon;
    public TextMeshProUGUI textTitle;   // ชื่อ WBC
    public TextMeshProUGUI textBody;    // คำอธิบาย
    public TextMeshProUGUI textBadge;   // เช่น "★★★ vs Bacteria"
    public Image       tipPanel;        // กล่อง tip ด้านล่าง
    public TextMeshProUGUI textTip;     // ข้อความ tip
    
    [Header("Data Assets")]
    public Sprite[] wbcSprites;

    [Header("Navigation")]
    public Image[] dots;               // ลาก Dot1, Dot2, Dot3 ใส่
    public Button  btnNext;
    public TextMeshProUGUI btnNextText;

    // ── สีของแต่ละ card ──────────────────────
    Color[] accentColors = new Color[]
    {
        new Color(0.11f, 0.62f, 0.46f),  // Teal   — Neutrophil
        new Color(0.09f, 0.37f, 0.64f),  // Blue   — Macrophage
        new Color(0.73f, 0.46f, 0.09f),  // Amber  — Eosinophil
    };

    // ── ข้อมูลแต่ละ card ──────────────────────
    string[] titles = { "Neutrophil", "Macrophage", "Eosinophil" };

    string[] bodies =
    {
        "ด่านหน้าที่เร็วที่สุด\nเชี่ยวชาญ Bacteria โดยตรง\nเป็นตัวแรกที่มาถึงจุดติดเชื้อ",
        "กินและย่อยเชื้อได้ แต่ช้ากว่า\nส่งสัญญาณต่อให้ T-cell ได้\nมีบทบาทสำคัญหลังเกมจบ",
        "เชี่ยวชาญ Parasite โดยเฉพาะ\nใช้กับ Bacteria แทบไม่ได้ผล\nเลือกผิด = เสีย resource ฟรี"
    };

    string[] badges  = { "3Stars vs Bacteria", "2Stars vs Bacteria", "1Star vs Bacteria" };

    string[] tips =
    {
        "ใช้กับ Bacteria = ได้ผลดีที่สุด",
        "ส่ง signal → T-cell จะช่วยทีหลัง",
        "เก็บไว้ใช้กับ Parasite เท่านั้น"
    };

    // ─────────────────────────────────────────
    int currentCard = 0;

    void Start()
    {
        ShowCard(0);
    }

    public void OnNextPressed()
    {
        currentCard++;

        if (currentCard >= titles.Length)
        {
            // อ่านครบแล้ว → ไป Diagnosis
            SceneManager.LoadScene("Diagnosis");
        }
        else
        {
            ShowCard(currentCard);
        }
    }

    void ShowCard(int index)
    {
        Color c = accentColors[index];

        if (index < wbcSprites.Length)
        {
        mainIcon.sprite = wbcSprites[index];
        }

        // ── ใส่สี ──
        accentBar.color  = c;
        iconCircle.color = new Color(c.r, c.g, c.b, 0.2f);

        // ── ใส่ข้อความ ──
        textTitle.text = titles[index];
        textTitle.color = c;
        textBody.text  = bodies[index];
        textBadge.text = badges[index];
        textBadge.color = c;
        textTip.text   = tips[index];

        // ── อัพเดต dots ──
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].color = (i == index)
                ? c
                : new Color(0.2f, 0.2f, 0.4f);

            // dot ปัจจุบันใหญ่กว่าหน่อย
            float size = (i == index) ? 28f : 20f;
            dots[i].rectTransform.sizeDelta = new Vector2(size, size);
        }

        // ── เปลี่ยนข้อความปุ่ม ──
        btnNextText.text = (index == titles.Length - 1)
            ? "เริ่มเกมเลย!"
            : "ถัดไป →";
    }
}