using UnityEngine;

public class ImmuneAttack : MonoBehaviour
{
    public PathogenType targetType;
    public float damage = 1f;

    void OnCollisionEnter2D(Collision2D col)
    {
        Pathogen pathogen = col.gameObject.GetComponent<Pathogen>();
        if (pathogen == null) return;

        if (pathogen.type == targetType)
        {
            pathogen.TakeDamage(damage); // Die() จัดการ score เอง
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}