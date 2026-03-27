using UnityEngine;

public class Pathogen : MonoBehaviour
{
    public PathogenType type;
    public float maxHp = 1f;
    public int scoreValue = 10;
    public float atpReward = 20f;
    public bool isBoss = false; // ← ติ๊กใน Inspector บน Boss prefab

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
    ATPManager.Instance?.AddATP(atpReward);

    if (isBoss)
        GameManager.Instance?.ShowVictory(); // เรียกก่อน

    Destroy(gameObject);
}
}