using UnityEngine;
using TMPro;
using UnityEngine.UI;

// วางบน GameObject "Managers"
// ใน Inspector ลาก Image[] ของ icon สายฟ้ามาใส่ lightningIcons (เหมือน heartImages)
public class ATPManager : MonoBehaviour
{
    public static ATPManager Instance;

    [Header("ATP Settings")]
    public float maxATP         = 100f;
    public float currentATP     = 50f;
    public float regenPerSecond = 0f;      // ปิด passive regen — ใช้ timer แทน

    [Header("Timed Regen (ทุก N วิ เพิ่ม 1 charge)")]
    public float regenInterval = 5f;       // เพิ่มทุก 5 วิ
    public float atpPerInterval = 20f;     // เพิ่มครั้งละ 1 charge (20 ATP)

    [Header("Kill Bonus")]
    public float atpPerKill = 20f;         // ยิงถูก → เพิ่ม 1 charge

    [Header("Lightning Icon UI (เหมือนหัวใจ)")]
    public Image[] lightningIcons;         // ลาก Image สายฟ้าทั้งหมดมาใส่
    public Sprite  lightningFull;          // icon สายฟ้าสว่าง
    public Sprite  lightningEmpty;         // icon สายฟ้าหรี่/ว่าง

    [Header("Cell Costs (ต้องตรงกับ DragCell.atpCost)")]
    // CD8=20, Macrophage=25, Eosinophil=30
    // 1 icon = 20 ATP → CD8=1, Macrophage=1.25, Eosinophil=1.5 charge

    float regenTimer = 0f;

    // ATP สูงสุดคำนวณจากจำนวน icon
    float MaxATPFromIcons => lightningIcons != null ? lightningIcons.Length * 20f : maxATP;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        maxATP = MaxATPFromIcons;
        UpdateUI();
    }

    void Update()
    {
        // Timed regen ทุก 5 วิ
        regenTimer += Time.deltaTime;
        if (regenTimer >= regenInterval)
        {
            regenTimer = 0f;
            AddATP(atpPerInterval);
        }
    }

    // เรียกเมื่อ kill enemy (ยิงถูก)
    public void OnKillBonus() => AddATP(atpPerKill);

    public void AddATP(float amount)
    {
        currentATP = Mathf.Min(currentATP + amount, MaxATPFromIcons);
        UpdateUI();
    }

    public bool TrySpend(float amount)
    {
        if (currentATP < amount) return false;
        currentATP -= amount;
        UpdateUI();
        return true;
    }

    void UpdateUI()
    {
        if (lightningIcons == null) return;

        // แต่ละ icon = 20 ATP — ถ้า currentATP ถึง threshold → สว่าง
        for (int i = 0; i < lightningIcons.Length; i++)
        {
            if (lightningIcons[i] == null) continue;
            float threshold = (i + 1) * 20f;
            lightningIcons[i].sprite = (currentATP >= threshold) ? lightningFull : lightningEmpty;
        }
    }
}