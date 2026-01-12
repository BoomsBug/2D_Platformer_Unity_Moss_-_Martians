using UnityEngine;

public class MovingPlatScript : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            collisionInfo.collider.transform.parent = transform;
        }
    }

    void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            collisionInfo.collider.transform.parent = null;
        }
    }
}
