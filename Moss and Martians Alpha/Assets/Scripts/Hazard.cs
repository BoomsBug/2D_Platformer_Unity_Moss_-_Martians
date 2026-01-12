using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerManager pm = other.GetComponent<PlayerManager>();
        PlayerMovement move = other.GetComponent<PlayerMovement>();

        if (pm != null)
            pm.TakeDamage(damage);

        if (move != null)
            move.ApplyKnockback(transform.position);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerManager pm = other.GetComponent<PlayerManager>();
        if (pm != null)
            pm.TakeDamage(damage);
    }
}
