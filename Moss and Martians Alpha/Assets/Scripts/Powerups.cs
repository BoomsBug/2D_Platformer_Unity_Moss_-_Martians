using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerupType { Health, Damage, SeedShot }
    public PowerupType type;
    public int DamageAmount = 1;
    public int HealAmount = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerManager playerManager = other.GetComponent<PlayerManager>();
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

        if (playerManager != null)
        {
            switch (type)
            {
                case PowerupType.Health:
                    playerManager.Heal(HealAmount);
                    break;
                case PowerupType.Damage:
                    playerMovement.IncreaseDamage(DamageAmount);
                    break;
                case PowerupType.SeedShot:
                    playerManager.EnableSeedShot();
                    break;
            }
            Destroy(gameObject);
        }
    }

}
