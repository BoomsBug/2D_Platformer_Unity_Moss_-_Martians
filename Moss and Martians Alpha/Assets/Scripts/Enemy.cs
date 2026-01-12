using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public Transform pointA;
    public Transform pointB;
    private Vector3 target;
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    [SerializeField] private SpriteRenderer enemyRenderer;
    [SerializeField] private float flashDuration = 0.1f;



    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        target = pointB.position;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        // Switch target when destination reached
        if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
            target = pointB.position;
        else if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
            target = pointA.position;
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        StartCoroutine(HurtFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private IEnumerator HurtFlash()
    {
        Color original = enemyRenderer.color;
        enemyRenderer.color = Color.red;

        yield return new WaitForSeconds(flashDuration);

        enemyRenderer.color = original;
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}

