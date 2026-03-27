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

    [Header("Narrative Timing")]
    public float narrativeDuration = 4.5f;

    [Header("References")]
    public LaneManager laneManager;

    [Header("Wave Narrative")]
    public string[] waveNarratives = {
        "🦠 แบคทีเรียเริ่มบุกรุก! ส่ง Neutrophil ไปสกัดกั้นด่านหน้า",
        "📈 แบคทีเรียแบ่งตัวเร็วขึ้น! นำ Macrophage มาช่วยจับกินและส่งสัญญาณ",
        "🛡️ เชื้อสร้าง Biofilm มาป้องกัน! ใช้ NK Cell ที่มีพลังทำลายสูงไปจัดการ",
        "⚠️ การติดเชื้อรุนแรงขึ้น! รักษาระดับแนวป้องกันไว้ให้มั่น",
        "💀 พบบอสใหญ่ (Superbug)! ระดมกำลังทั้งหมดที่มี!"
    };
    [Header("Wave Narrative UI")]
public TextMeshProUGUI waveNarrativeText;
    int currentWave = -1;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (waveBannerText != null) waveBannerText.gameObject.SetActive(false);
        if (waveNarrativeText != null) waveNarrativeText.gameObject.SetActive(false);

        currentWave = 0;
        ApplyWaveConfig(0);

        StartCoroutine(ShowWaveBanner(1));
    }
    // ================= MAIN =================

    public void CheckWave(int score)
    {
        int newWave = 0;

        for (int i = waveScoreThresholds.Length - 1; i >= 0; i--)
        {
            if (score >= waveScoreThresholds[i])
            {
                newWave = i;
                break;
            }
        }

        if (newWave > currentWave)
        {
            StartCoroutine(TransitionWave(newWave));
        }
    }

IEnumerator TransitionWave(int newWave)
{
    // 🔴 หยุด spawn
    laneManager.isStopSpawning = true;
    laneManager.StopAllCoroutines();

    // 🔵 แสดง Narrative (ช่องใหม่)
    yield return StartCoroutine(ShowNarrative(currentWave + 1));

    yield return new WaitForSecondsRealtime(0.5f);

    // 🔄 เปลี่ยน wave
    currentWave = newWave;
    ApplyWaveConfig(currentWave);

    // 🟡 แสดง WAVE X (อีกช่อง)
    yield return StartCoroutine(ShowWaveBanner(currentWave + 1));

    // 🔥 เริ่ม spawn หลังสุด
    laneManager.ResetForNewWave();

    // 👑 boss
    if (currentWave == 4)
        laneManager.SetBossWave();
}
    // ================= UI =================

    IEnumerator ShowNarrative(int waveNumber)
    {
        if (waveNarrativeText == null) yield break;

        int idx = waveNumber - 1;
        if (idx >= waveNarratives.Length) yield break;

        waveNarrativeText.text = waveNarratives[idx];
        waveNarrativeText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(narrativeDuration);

        waveNarrativeText.gameObject.SetActive(false);
    }

    IEnumerator ShowWaveBanner(int waveNumber)
    {
        if (waveBannerText == null) yield break;

        waveBannerText.text = $"— WAVE {waveNumber} —";
        waveBannerText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(1.5f);

        waveBannerText.gameObject.SetActive(false);
    }

    // ================= UTIL =================

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
        laneManager.rounds        = c.rounds;
    }
}

// ================= CONFIG =================

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