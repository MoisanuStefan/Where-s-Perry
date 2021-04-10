using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapdoor : MonoBehaviour
{
    public float moveDistance;
    public bool isTriggered;
    private bool isMoving;
    public float lerpTime;
    public bool isVertical;

    private Vector3 unTriggeredPosition;
    private Vector3 triggeredPosition;

    private Vector3 destination;
    private float perc = 1f;
    private float incrementAmount;


    // Start is called before the first frame update
    void Start()
    {
        unTriggeredPosition = transform.position;
        triggeredPosition = transform.position;
        if (isVertical)
        {
            triggeredPosition += Vector3.up * moveDistance; 
        }
        else
        {
            triggeredPosition += Vector3.left * moveDistance;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (perc < 1f)
            {
                perc += Time.deltaTime * incrementAmount;
                transform.position = Vector3.Lerp(transform.position, destination, perc);
            }
            else
            {
                isMoving = false;
                transform.position = destination;
            }
        }
    }

    private void FixedUpdate()
    {
       
    }

    public void Trigger()
    {
        perc = 0f;
        incrementAmount = 1f / lerpTime;

        if (isTriggered)
        {
            destination = unTriggeredPosition;
        }
        else
        {
            destination = triggeredPosition;
        }
        isMoving = true;
        isTriggered = !isTriggered;
    }
}
