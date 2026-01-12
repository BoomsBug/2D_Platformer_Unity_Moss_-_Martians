
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenOnTrigger : MonoBehaviour
{
    // Tracks movement
    public Vector3 movementTracker = Vector3.zero;

    public float time = 0.2f;

    private Vector3 startPosition;

    // start + movement
    private Vector3 endPosition;

    // target
    public Vector3 target;

    // velocity starts at zero
    private Vector3 velocity = Vector3.zero;

    private bool moveUpNow = false;

    void Start()
    {
        startPosition = transform.position;
        endPosition = transform.position + movementTracker;

    }

    void Update()
    {
        if (moveUpNow)
        {
            target = endPosition;
        } else
        {
            target = startPosition;
        }
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, time);
            

    }

    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            moveUpNow = true;
            velocity = Vector3.zero;
        }
    }

    void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            moveUpNow = false;
            velocity = Vector3.zero;
        }
    }
}
