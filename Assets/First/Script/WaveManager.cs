using UnityEngine;
using TMPro;
using System.Collections;

// วางสคริปต์นี้บน GameObject ว่างๆ ใน Scene แล้ว link LaneManager ใน Inspector
public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Wave Thresholds (Score)")]
    public int[] waveScoreThresholds = { 0, 100, 250, 500, 1000 };

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
        CheckWave(0);
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
        }
    }

    void ApplyWaveConfig(int idx)
    {
        if (waveConfigs == null || idx >= waveConfigs.Length || laneManager == null) return;
        WaveConfig c = waveConfigs[idx];
        laneManager.minSpawnGap   = c.minSpawnGap;
        laneManager.maxSpawnGap   = c.maxSpawnGap;
        laneManager.roundInterval = c.roundInterval;
        laneManager.pathogenSpeed = c.pathogenSpeed;
        laneManager.swarmCount    = c.swarmCount;
    }

    IEnumerator ShowWaveBanner(int waveNumber)
    {
        if (waveBannerText == null) yield break;
        waveBannerText.text = $"— WAVE {waveNumber} —";
        waveBannerText.gameObject.SetActive(true);
        yield return new WaitForSeconds(bannerDuration);
        waveBannerText.gameObject.SetActive(false);
    }

    public int GetCurrentWave() => currentWave + 1;
}

[System.Serializable]
public class WaveConfig
{
    public float minSpawnGap   = 1.5f;
    public float maxSpawnGap   = 2.5f;
    public float roundInterval = 5f;
    public float pathogenSpeed = 2f;
    [Tooltip("1 = ปกติ, 2+ = Swarm")]
    public int   swarmCount    = 1;
}