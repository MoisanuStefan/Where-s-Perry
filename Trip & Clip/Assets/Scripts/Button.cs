using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public float moveDistance;
    public float lerpTime;

    public VTrapdoor trapdoor;

    private bool isTriggered;
    private bool isColliding;

    private bool isMoving;
    private bool isDisabled;

    private float disableTime = 0.2f;
    private float elapsedDisabledTime = 0f;

    private Vector3 unTriggeredPosition;
    private Vector3 triggeredPosition;

    // Start is called before the first frame update
    void Start()
    {
        unTriggeredPosition = transform.position;
        triggeredPosition = transform.position + Vector3.up * moveDistance;
    } 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDisabled)
        {
            elapsedDisabledTime += Time.deltaTime;
            if (disableTime - elapsedDisabledTime <= 0)
            {
                isDisabled = false;
                elapsedDisabledTime = 0;
            }
        }


        if (!isDisabled)
        {
            if (!isColliding && isTriggered || isColliding && !isTriggered)
            {
                isDisabled = true;
                Trigger();
            }

        }
    }

   

    void Trigger()
    {
        Vector3 destination;
        trapdoor.Trigger();

        destination = (isTriggered) ? unTriggeredPosition : triggeredPosition;

        isMoving = true;
        isTriggered = !isTriggered;
        StartCoroutine(Move(0f, destination));
    }

    IEnumerator Move(float perc, Vector3 destination)
    {
        float incrementAmount = 1f / lerpTime;
        if (isMoving)
        {
            while (perc < 1f)
            {
                Debug.Log(perc);
                perc += Time.deltaTime * incrementAmount;
                transform.position = Vector3.Lerp(transform.position, destination, perc);
                
            }
            if (perc >= 1f)
            {
                isMoving = false;
                transform.position = destination;
            }
        }
        yield return null;
    }

}
