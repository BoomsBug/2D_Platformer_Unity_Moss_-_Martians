using UnityEngine;

public class SeedProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public float destroyAfterSeconds = 1f;

    private Vector2 direction;
    private Rigidbody2D rb;

    private void Awake()
    {
        // Cache the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 shootDirection)
    {
        direction = shootDirection.normalized;

        rb.linearVelocity = direction * speed;

        Destroy(gameObject, destroyAfterSeconds);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
        }
    }

}
