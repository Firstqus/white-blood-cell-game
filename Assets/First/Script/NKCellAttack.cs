using UnityEngine;

public class NKCellAttack : MonoBehaviour
{
    public float damage = 2f; // ฆ่า Armored (HP=2) ได้ทีเดียว

    void OnCollisionEnter2D(Collision2D col)
    {
        Pathogen p = col.gameObject.GetComponent<Pathogen>();
        if (p == null) return;

        p.TakeDamage(damage);
        Destroy(gameObject);
    }
}