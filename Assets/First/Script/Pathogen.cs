using UnityEngine;

public class Pathogen : MonoBehaviour
{
    public PathogenType type;
    public float maxHp = 1f;
    public int scoreValue = 10;
    public float atpReward = 20f; // ← ตั้งต่อ prefab: Normal=20, Armored=50, Boss=100

    float currentHp;

    void Awake() => currentHp = maxHp;

    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0) Die();
    }

    void Die()
    {
        GameManager.Instance?.AddScore(scoreValue);
        ATPManager.Instance?.AddATP(atpReward); // ← คืน ATP โดยตรง ไม่ผ่าน OnKillBonus
        Destroy(gameObject);
    }
}