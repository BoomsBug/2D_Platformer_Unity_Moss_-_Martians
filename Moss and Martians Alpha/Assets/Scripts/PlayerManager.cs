using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    [Header("general")]
    public SpriteRenderer frontRenderer;
    public SpriteRenderer sideRenderer;
    private PlayerMovement playerMovement;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private GameObject player;

    [Header("Seed Shoot Ability")]
    public bool seedShotEnabled = false;
    public GameObject seedProjectilePrefab;
    public Transform shootPoint;          
    public float shootCooldown = 0.5f;
    private float shootTimer;

    [Header("Seed Shoot Audio")]
    [SerializeField] private AudioSource seedAudioSource;
    [SerializeField] private AudioClip seedShotSfx;


    [Header("player dying and respawning")]
    [SerializeField] private float invulnTime = 1f;
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    private float invulnTimer;
    [SerializeField] private int flashCount = 5;
    private static Vector3 respawnPosition;
    public static bool checkpointHit = false;


    private void Start()
    {
        if (checkpointHit == true)
        {
            Respawn();
        }
        currentHealth = maxHealth;
        UpdateHearts();
        respawnPosition = transform.position;
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (invulnTimer > 0f)
        {
            invulnTimer -= Time.unscaledDeltaTime;
        }

        shootTimer -= Time.deltaTime;
        if (seedShotEnabled && Input.GetKeyDown(KeyCode.F) && shootTimer <= 0f)
        {
            ShootSeed();
        }
    }

    public void TakeDamage(int amount)
    {
        if (invulnTimer > 0f || currentHealth <= 0) return;

        currentHealth -= amount;
        invulnTimer = invulnTime;
        UpdateHearts();
        StartCoroutine(HurtFlash());

        if (currentHealth <= 0)
        {
            if (levelManager != null)
            {
                levelManager.PlayerDied();
            }
        }
    }

    private IEnumerator HurtFlash()
    {
        Color original = frontRenderer.color;
        Color hurtColor = Color.red;

        float segment = invulnTime / (flashCount * 2f); 
        
        for (int i = 0; i < flashCount; i++)
        {
            frontRenderer.color = hurtColor;
            sideRenderer.color = hurtColor;
            yield return new WaitForSecondsRealtime(segment);

            frontRenderer.color = original;
            sideRenderer.color = original;
            yield return new WaitForSecondsRealtime(segment);
        }

        frontRenderer.color = original;
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            bool isAlive = i < currentHealth;
            if (hearts[i] != null) hearts[i].SetActive(isAlive);
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth >= maxHealth) return;
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHearts();
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        respawnPosition = checkpointPosition;
        checkpointHit = true;
    }

    public void Respawn()
    {
        player.transform.position = respawnPosition;
        currentHealth = maxHealth;
        invulnTimer = 0f;
        UpdateHearts();
    }

    public void EnableSeedShot()
    {
        seedShotEnabled = true;
    }

    private void ShootSeed()
    {
        if (seedAudioSource != null && seedShotSfx != null)
        {
            seedAudioSource.PlayOneShot(seedShotSfx);
        }

        Vector2 direction = Vector2.right;
        if (playerMovement.sideRenderer.flipX) direction = Vector2.left;

        float spawnDistance = 0.6f;
        Vector3 spawnPos = transform.position + (Vector3)direction * spawnDistance;

        GameObject seed = Instantiate(seedProjectilePrefab, spawnPos, Quaternion.identity);
        SeedProjectile seedScript = seed.GetComponent<SeedProjectile>();
        seedScript.Initialize(direction);

        shootTimer = shootCooldown;
    }

}
