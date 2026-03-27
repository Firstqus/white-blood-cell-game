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

// ไฟล์: WaveManager.cs

    public void CheckWave(int score)
    {
        int newWave = 0;
        for (int i = waveScoreThresholds.Length - 1; i >= 0; i--)
        {
            if (score >= waveScoreThresholds[i]) { newWave = i; break; }
        }

        if (newWave > currentWave)
        {
            currentWave = newWave;
            ApplyWaveConfig(currentWave);
            
            StartCoroutine(ShowWaveBanner(currentWave + 1));
            laneManager.ResetForNewWave();
            
            // ถ้าเข้า Wave 5 (Index 4) ให้บอก LaneManager ว่านี่คือรอบบอส
            if (currentWave == 4) 
            {
                laneManager.SetBossWave();
            }
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

// ไฟล์: WaveManager.cs (ส่วนที่ปรับปรุงใหม่)

[Header("Wave Narrative")]
public string[] waveNarratives = {
    "🦠 แบคทีเรียเริ่มบุกรุก! ส่ง Neutrophil ไปสกัดกั้นด่านหน้า",
    "📈 แบคทีเรียแบ่งตัวเร็วขึ้น! นำ Macrophage มาช่วยจับกินและส่งสัญญาณ",
    "🛡️ เชื้อสร้าง Biofilm มาป้องกัน! ใช้ NK Cell ที่มีพลังทำลายสูงไปจัดการ",
    "⚠️ การติดเชื้อรุนแรงขึ้น! รักษาระดับแนวป้องกันไว้ให้มั่น",
    "💀 พบบอสใหญ่ (Superbug)! ระดมกำลังทั้งหมดที่มี!"
};

IEnumerator ShowWaveBanner(int waveNumber)
{
    if (waveBannerText == null) yield break;

    int idx = waveNumber - 1;
    // ดึง Narrative ตาม Wave
    string narrative = idx < waveNarratives.Length ? waveNarratives[idx] : "";

    // ข้อความปลดล็อก (อิงตามที่คุยกันรอบก่อน)
    string unlockMsg = waveNumber switch
    {
        2 => "\n[NEW] 🟡 Macrophage: กินแบคทีเรียได้ต่อเนื่อง",
        3 => "\n[NEW] 🔵 NK Cell: ทะลวงเกราะ Biofilm ได้ดี",
        _ => ""
    };

    waveBannerText.text = $"— WAVE {waveNumber} —\n{narrative}\n<color=yellow>{unlockMsg}</color>";
    waveBannerText.gameObject.SetActive(true);
    
    // หยุดเวลาเกมชั่วคราวเพื่อให้ผู้เล่นอ่าน (Optional)
    // Time.timeScale = 0f; 
    yield return new WaitForSecondsRealtime(bannerDuration);
    // Time.timeScale = 1f;

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