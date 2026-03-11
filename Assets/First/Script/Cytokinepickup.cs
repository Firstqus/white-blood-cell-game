using UnityEngine;
using System.Collections;

// แขวนสคริปต์นี้กับ Prefab ของ Power-up แต่ละชนิด
public class CytokinePickup : MonoBehaviour
{
    [Header("Type")]
    public CytokineType cytokineType = CytokineType.Disinfectant;

    [Header("Movement")]
    public float fallSpeed = 1.5f;
    public float lifetime  = 8f;

    [Header("SpeedDown Config")]
    public float slowDuration = 5f;
    public float slowFactor   = 0.4f;

    [Header("ATPBoost Config")]
    public float atpBoostAmount = 40f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    void OnMouseDown()
    {
        Collect();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerZone"))
            Collect();
    }

    void Collect()
    {
        switch (cytokineType)
        {
            case CytokineType.Disinfectant: ClearAllPathogens();                    break;
            case CytokineType.Vitamin:      HealPlayer();                            break;
            case CytokineType.ATPBoost:     BoostATP();                              break;
            case CytokineType.SpeedDown:    StartCoroutine(SlowAllPathogens());     break;
        }
        Destroy(gameObject);
    }

    void ClearAllPathogens()
    {
        Pathogen[] all = FindObjectsByType<Pathogen>(FindObjectsSortMode.None);
        foreach (Pathogen p in all)
        {
            GameManager.Instance?.AddScore(5);
            Destroy(p.gameObject);
        }
    }

    void HealPlayer()
    {
        GameManager.Instance?.Heal(1);
    }

    void BoostATP()
    {
        ATPManager.Instance?.AddATP(atpBoostAmount);
    }

    IEnumerator SlowAllPathogens()
    {
        VirusMove[] movers = FindObjectsByType<VirusMove>(FindObjectsSortMode.None);
        float[] ms = new float[movers.Length];

        for (int i = 0; i < movers.Length; i++) { ms[i] = movers[i].speed; movers[i].speed *= slowFactor; }

        yield return new WaitForSeconds(slowDuration);

        for (int i = 0; i < movers.Length; i++) if (movers[i]) movers[i].speed = ms[i];
    }
}