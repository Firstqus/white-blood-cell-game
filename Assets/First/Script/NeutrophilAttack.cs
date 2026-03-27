using UnityEngine;

public class NeutrophilAttack : MonoBehaviour
{
    public float splashRadius = 1.5f;
    public float damage       = 1f;

    void OnCollisionEnter2D(Collision2D col)
    {
        // ต้องชนกับ Pathogen ก่อนถึงจะ splash
        Pathogen hit = col.gameObject.GetComponent<Pathogen>();
        if (hit == null) return;

        // Splash โดนทุกตัวในรัศมี
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, splashRadius);
        foreach (var c in nearby)
        {
            Pathogen p = c.GetComponent<Pathogen>();
            if (p != null) p.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    // Debug — เห็นรัศมี splash ใน Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}