﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public float moveDistance;
    public float lerpTime;
    public float delayTime = 0;
    public bool isVertical;

    public Trapdoor trapdoor;

    private bool isTriggered;
    private bool isColliding;
    private bool isMoving;
    private bool isDisabled;

    private float disableTime = 0.2f;
    private float elapsedDisabledTime = 0f;
    private float elapsedTriggerDelay= 0f;
    private float triggerTime;

    private Vector3 unTriggeredPosition;
    private Vector3 triggeredPosition;



    // Start is called before the first frame update
    void Start()
    {
        unTriggeredPosition = transform.position;
        triggeredPosition = transform.position;
        if (isVertical)
        {
            triggeredPosition += Vector3.left * moveDistance;
        }
        else
        {
            triggeredPosition += Vector3.up * moveDistance;

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
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
