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
        new Color(0.73f, 0.46f, 0.09f),  // Amber  — Nk cell
    };

    // ── ข้อมูลแต่ละ card ──────────────────────
    string[] titles = { "Neutrophil", "Macrophage", "NK Cell" };

    string[] bodies =
    {
        "ด่านหน้า\nเซลล์แนวหน้าที่ถึงจุดติดเชื้อก่อนใคร จับกินและทำลายแบคทีเรียทันทีด้วย Phagocytosis ราคาถูก ใช้ได้เรื่อยๆ แต่อึดน้อย",
        "ตัวถึก เรียกเพื่อนได้\nเซลล์ขนาดใหญ่ที่จับกินเชื้อโรคได้ต่อเนื่องและทนทานกว่า Neutrophil นอกจากนี้ยังส่งสัญญาณ Cytokine เรียก T-Cell เข้ามาเสริมแนวรับ",
        "ดาเมจสูง ไม่ต้องรอคำสั่ง\nนักฆ่าของระบบภูมิคุ้มกัน สั่งให้เซลล์ติดเชื้อทำลายตัวเอง (Apoptosis) ได้โดยอัตโนมัติ เหมาะกับศัตรูถึกและบอส"
    };

    string[] badges  = { "3Stars vs Bacteria", "2Stars vs Bacteria", "1Star vs Bacteria" };

    string[] tips =
    {
        "ใช้ต่อสู้กับ Bacteria ที่มาเป็นกลุ่มๆได้ดี จะระเบิดตัวเอง",
        "ใช้ต่อสู้กับ Bacteria ที่มาเดี่ยวหรือปกติทั่วไป แล้วจะส่งสัญญาณไปหา T-cell",
        "ใช้ต่อสู้กับ Bacteria ที่ตัวใหญ่ หรือมีความอันตรายสูง"
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