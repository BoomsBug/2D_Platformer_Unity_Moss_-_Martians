using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween: MonoBehaviour 
{
    // Tracks movement
    public Vector3 movementTracker = Vector3.zero;

    public float time = 0.2f;

    private Vector3 startPosition;

    // start + movement
    private Vector3 endPosition;

    // velocity starts at zero
    private Vector3 velocity = Vector3.zero;

    private bool areWeMovingToTarget = true;

    public bool cyclic = false;

    void Start()
    {
        startPosition = transform.position;
        endPosition = transform.position + movementTracker;

    }

    void Update()
    {
        if (areWeMovingToTarget)
        {
            transform.position = Vector3.SmoothDamp(transform.position, endPosition, ref velocity, time);
            if ((transform.position - endPosition).sqrMagnitude < 0.1f)
            {
                areWeMovingToTarget = false;
            }
        }
        else if(cyclic) 
        {
            transform.position = Vector3.SmoothDamp(transform.position, startPosition, ref velocity, time);
            if ((transform.position - startPosition).sqrMagnitude < 0.1f)
            {
                areWeMovingToTarget = true;
            }
        }

    }

}

