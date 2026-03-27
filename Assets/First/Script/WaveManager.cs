using UnityEngine;
using TMPro;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Wave Thresholds (Score)")]
   public int[] waveScoreThresholds = { 0, 50, 130, 200, 250 }; 

    [Header("Per-Wave Config")]
    public WaveConfig[] waveConfigs;

    [Header("Wave Banner UI")]
    public TextMeshProUGUI waveBannerText;
    public float bannerDuration = 2.5f;

    [Header("References")]
    public LaneManager laneManager;

    int currentWave = -1;
    
    

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

void Start()
{
    if (waveBannerText != null) waveBannerText.gameObject.SetActive(false);
    currentWave = 0;          // ← เริ่มที่ 0 แทน -1
    ApplyWaveConfig(0);
    StartCoroutine(ShowWaveBanner(1));
}

public void CheckWave(int score)
{
    int newWave = 0;
    for (int i = waveScoreThresholds.Length - 1; i >= 0; i--)
    {
        if (score >= waveScoreThresholds[i]) { newWave = i; break; }
    }

    if (newWave != currentWave)
    {
        currentWave = newWave;
        ApplyWaveConfig(currentWave);
        StartCoroutine(ShowWaveBanner(currentWave + 1));
        GameManager.Instance?.UpdateCellUnlock(currentWave + 1); // ← เรียกตรงนี้ด้วย
    }
}

    public int GetCurrentWave() => currentWave + 1 < 1 ? 1 : currentWave + 1;

    void ApplyWaveConfig(int idx)
    {
        if (waveConfigs == null || idx >= waveConfigs.Length || laneManager == null) return;
        WaveConfig c = waveConfigs[idx];
        laneManager.minSpawnGap   = c.minSpawnGap;
        laneManager.maxSpawnGap   = c.maxSpawnGap;
        laneManager.roundInterval = c.roundInterval;
        laneManager.pathogenSpeed = c.pathogenSpeed;
        laneManager.swarmCount    = c.swarmCount;
        laneManager.rounds        = c.rounds; // ← เปลี่ยนจาก roundVariants
    }

    [Header("Wave Description")]
    public string[] waveDescriptions = {
        "🦠 Bacteria เริ่มเข้าแผล กำลังแบ่งตัว!",
        "⚡ Bacteria กระจายเร็วขึ้น ระวัง!",
        "🛡 บางตัวสร้าง Biofilm ป้องกันตัวเอง!",
        "🦠🦠 Bacteria รวมกลุ่ม — Biofilm Cluster!",
        "💀 เชื้อดื้อยาปรากฏ! ระดมทุกอย่าง!"
    };

IEnumerator ShowWaveBanner(int waveNumber)
{
    if (waveBannerText == null) yield break;

    int idx = waveNumber - 1;
    string desc = idx < waveDescriptions.Length ? waveDescriptions[idx] : "";

    // ปรับเงื่อนไขการแสดงข้อความ Unlocked ให้ตรงกับ Logic ใหม่
    string unlockMsg = waveNumber switch
    {
        2 => "\n🟡 Macrophage Unlocked! Neutrophil ส่งสัญญาณเรียกมาแล้ว",
        3 => "\n🔵 NK Cell Unlocked! ระบบส่งผู้เชี่ยวชาญมาช่วย",
        _ => ""
    };

    waveBannerText.text = $"— WAVE {waveNumber} —\n{desc}{unlockMsg}";
    waveBannerText.gameObject.SetActive(true);
    yield return new WaitForSeconds(bannerDuration);
    waveBannerText.gameObject.SetActive(false);
}

}

[System.Serializable]
public class WaveConfig
{
    public float minSpawnGap   = 1.5f;
    public float maxSpawnGap   = 2.5f;
    public float roundInterval = 5f;
    public float pathogenSpeed = 2f;
    public int   swarmCount    = 1;

    [Header("Round Schedule")]
    public SpawnRound[] rounds = {
        new SpawnRound { variant = BacteriaVariant.Normal },
        new SpawnRound { variant = BacteriaVariant.Fast,    speedOverride = 5f },
        new SpawnRound { variant = BacteriaVariant.Armored, hpOverride    = 3f },
        new SpawnRound { variant = BacteriaVariant.Swarm,   swarmSize     = 4  },
        new SpawnRound { variant = BacteriaVariant.Boss,    hpOverride    = 5f },
    };
}