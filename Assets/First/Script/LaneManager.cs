using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class LaneManager : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform spawnLane1, spawnLane2, spawnLane3;

    [Header("Prefabs")]
    public GameObject bacteriaPrefab;
    public GameObject bacteriaFastPrefab;
    public GameObject bacteriaArmorPrefab;
    public GameObject bacteriaBossPrefab;

    [Header("Hint UI")]
    public TextMeshProUGUI hintText;
    public float hintDuration    = 2f;
    public float delayAfterHints = 2f;

    [Header("Spawn Timing (ปรับโดย WaveManager)")]
    public float minSpawnGap   = 1f;
    public float maxSpawnGap   = 2.5f;
    public float roundInterval = 5f;
    public float pathogenSpeed = 2f;
    public int   swarmCount    = 1;

    [Header("Round Schedule (ตั้งแต่ละ round ใน Inspector)")]
    public SpawnRound[] rounds;

    Transform[] spawnPoints;
    int currentRound = 0;

    void Start()
    {
        spawnPoints = new Transform[] { spawnLane1, spawnLane2, spawnLane3 };
        StartCoroutine(GameSequence());
    }

    IEnumerator GameSequence()
    {
        yield return StartCoroutine(ShowHint());
        yield return new WaitForSeconds(delayAfterHints);
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnOneByOne());
            yield return new WaitForSeconds(roundInterval);
        }
    }

    IEnumerator SpawnOneByOne()
    {
        // อ่าน round ปัจจุบันจาก rounds[] ตรงๆ ไม่สุ่ม
        SpawnRound round = GetCurrentRound();

        // Shuffle แค่ลำดับ lane ไม่ใช่ variant
        List<int> order = new List<int> { 0, 1, 2 };
        for (int i = order.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (order[i], order[j]) = (order[j], order[i]);
        }

        if (round.variant == BacteriaVariant.Boss)
        {
            SpawnAt(1, round);
            currentRound++;
            yield break;
        }

        foreach (int i in order)
        {
            if (spawnPoints[i] == null) continue;
            SpawnAt(i, round);
            yield return new WaitForSeconds(Random.Range(minSpawnGap, maxSpawnGap));
        }

        currentRound++;
    }

    void SpawnAt(int laneIdx, SpawnRound round)
    {
        GameObject prefab = round.variant switch
        {
            BacteriaVariant.Fast    => bacteriaFastPrefab  ?? bacteriaPrefab,
            BacteriaVariant.Armored => bacteriaArmorPrefab ?? bacteriaPrefab,
            BacteriaVariant.Boss    => bacteriaBossPrefab  ?? bacteriaPrefab,
            _                       => bacteriaPrefab
        };

        int count = round.variant == BacteriaVariant.Swarm ? round.swarmSize : 1;

        for (int s = 0; s < count; s++)
        {
            Vector3 pos = spawnPoints[laneIdx].position + Vector3.up * (s * 1.2f);
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);

            var mover = go.GetComponent<VirusMove>();
            if (mover != null) mover.speed = round.speedOverride > 0
                ? round.speedOverride : pathogenSpeed;

            var pathogen = go.GetComponent<Pathogen>();
            if (pathogen != null) pathogen.maxHp = round.hpOverride > 0
                ? round.hpOverride : 1f;
        }
    }

    SpawnRound GetCurrentRound()
    {
        if (rounds == null || rounds.Length == 0)
            return new SpawnRound(); // default

        return currentRound < rounds.Length
            ? rounds[currentRound]
            : rounds[rounds.Length - 1]; // loop round สุดท้าย
    }

    IEnumerator ShowHint()
    {
        if (hintText == null) yield break;

        SpawnRound round = GetCurrentRound();
        string msg = round.variant switch
        {
            BacteriaVariant.Fast    => "⚡ Fast Bacteria! Assign quickly!",
            BacteriaVariant.Armored => "🛡 Armored Bacteria! Send more Macrophages!",
            BacteriaVariant.Swarm   => "🦠 Swarm incoming! Cover all lanes!",
            BacteriaVariant.Boss    => "💀 BOSS! All hands on deck!",
            _                       => "🦠 Bacteria detected → use Macrophage"
        };

        hintText.text = msg;
        hintText.gameObject.SetActive(true);
        yield return new WaitForSeconds(hintDuration);
        hintText.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class SpawnRound
{
    public BacteriaVariant variant    = BacteriaVariant.Normal;
    [Tooltip("0 = ใช้ค่า pathogenSpeed จาก LaneManager")]
    public float           speedOverride = 0f;
    [Tooltip("0 = HP = 1 (ค่าเริ่มต้น)")]
    public float           hpOverride    = 0f;
    [Tooltip("ใช้เมื่อ variant = Swarm")]
    public int             swarmSize     = 3;
}
