using UnityEngine;

public class Pathogen : MonoBehaviour
{
    public PathogenType type;
    public float maxHp = 1f;
    public int scoreValue = 10;
    public float atpReward = 20f;
    public bool isBoss = false;

    [Header("Sound")]
    public AudioClip deathSound;     // ← ลาก SFX ปกติใส่ใน Inspector
    public AudioClip bossDeathSound; // ← ลาก SFX บอสใส่ใน Inspector

    float currentHp;

    void Awake() => currentHp = maxHp;

    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0) Die();
    }

    [Header("Effects")]
    public GameObject deathParticle; // ← ลาก Prefab ใส่ใน Inspector

    void Die()
    {
        if (deathParticle != null)
            Instantiate(deathParticle, transform.position, Quaternion.identity);

        // โค้ดเดิม...
        AudioClip clip = isBoss && bossDeathSound ? bossDeathSound : deathSound;
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, transform.position);

        GameManager.Instance?.AddScore(scoreValue);
        ATPManager.Instance?.AddATP(atpReward);

        if (isBoss) GameManager.Instance?.ShowVictory();

        Destroy(gameObject);
    }

    
}