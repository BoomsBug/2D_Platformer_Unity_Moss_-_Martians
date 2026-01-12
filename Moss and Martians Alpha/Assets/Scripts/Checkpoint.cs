using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    public SpriteRenderer preTouch;
    public SpriteRenderer postTouch;
    public static bool checkpointHit = false;

    private void Start()
    {
        if (checkpointHit == true)
        {
            preTouch.enabled = false;
            postTouch.enabled = true;
        }
        else
        {
            preTouch.enabled = true;
            postTouch.enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.SetCheckpoint(transform.position);
            }
            checkpointHit = true;
            preTouch.enabled = false;
            postTouch.enabled = true;
        }
    }
}

