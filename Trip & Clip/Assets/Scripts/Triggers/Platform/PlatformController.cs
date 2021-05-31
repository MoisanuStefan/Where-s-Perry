﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : Trigger
{
    public Vector3[] localWaypoints;

    public float speed;

    public bool isTriggerable;
    public bool isFromMap = false;

    private Vector3[] globalWaypoins;

    private float percentBetweenWaypoints;

    private int fromWaypointIndex;

    private Rigidbody2D rb;
    private GroundPlayerController groundPlayerController;
    private Vector3 platformDirection;
    private float xVelocity;
    private Vector3 newPos;
    private Vector3 oldPos;
    private Vector3 unTriggeredPosition;
    private Vector3 triggeredPosition;
    private float platformMovement;

    private bool isMoving;
    private bool isTriggered;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fromWaypointIndex = 0;
        globalWaypoins = new Vector3[localWaypoints.Length];
        for(int i = 0; i < globalWaypoins.Length; ++i)
        {
            globalWaypoins[i] = localWaypoints[i] + transform.position;
        }
        oldPos = transform.position;
        if (isTriggerable)
        {
            unTriggeredPosition = globalWaypoins[0];
            triggeredPosition = globalWaypoins[1];
        }
    }

    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, globalWaypoins[fromWaypointIndex + 1], 0.5f), speed * Time.deltaTime / Vector3.Distance(transform.position, globalWaypoins[fromWaypointIndex + 1]));
        //transform.position = Vector3.MoveTowards(transform.position, globalWaypoins[(fromWaypointIndex + 1) % globalWaypoins.Length], 0.5f);
        
        //UpdateHorizontalVelocity();
        /*
        if (playerOn)
        {
            groundPlayerController.SetPlatformXVelocity(xVelocity);
        }
        */
        

    }

    private void FixedUpdate()
    {
        if (isMoving || !isTriggerable)
        {
            transform.Translate(CalculatePlatformMovement());
        }


    }

    /*
    private void UpdateHorizontalVelocity()
    {
        newPos = transform.position;
        xVelocity = (newPos.x - oldPos.x) / Time.deltaTime;
        oldPos = newPos;
    }
    */
  
    /*
    private void SetPlatformDirection(Vector2 newPosition)
    {
        if (newPosition.x > transform.position.x)
        {
            platformDirection = Vector2.right;
            platformMovement = Mathf.Abs(newPosition.x - transform.position.x);
        }
        else if (newPosition.x < transform.position.x)
        {
            platformDirection = Vector2.left;
            platformMovement = Mathf.Abs(newPosition.x - transform.position.x);

        }
        else if (newPosition.y < transform.position.y)
        {
            platformDirection = Vector2.down;
            platformMovement = Mathf.Abs(newPosition.y - transform.position.y);

        }
        else
        {
            platformDirection = Vector2.up;
            platformMovement = Mathf.Abs(newPosition.y - transform.position.y);

        }

    }

    */
    public override void TriggerFunction()
    {
        if (isTriggerable)
        {
            base.TriggerFunction();
            isTriggered = !isTriggered;
            percentBetweenWaypoints = 0;
            isMoving = true;
        }
    }
    private Vector3 CalculatePlatformMovement()
    {
        Vector3 newPosition;
        if (isTriggerable || isFromMap)
        {
            Vector3 destination = (isTriggered) ? triggeredPosition : unTriggeredPosition;
            float distance = Vector3.Distance(transform.position, destination);
            percentBetweenWaypoints += Time.deltaTime * speed / distance;

            newPosition = Vector3.Lerp(transform.position, destination, percentBetweenWaypoints);
            if (1 - percentBetweenWaypoints < 0.1f) {
                percentBetweenWaypoints = 0;
                isMoving = false;
            
            }

        }
        else
        {
            int toWaypointIndex = fromWaypointIndex + 1;
            float distance = Vector3.Distance(transform.position, globalWaypoins[toWaypointIndex]);
            percentBetweenWaypoints += Time.deltaTime * speed / distance;

            newPosition = Vector3.Lerp(transform.position, globalWaypoins[toWaypointIndex], percentBetweenWaypoints);

            if (1 - percentBetweenWaypoints < 0.01f)
            {
                percentBetweenWaypoints = 0;
                fromWaypointIndex++;
                if (fromWaypointIndex >= globalWaypoins.Length - 1)
                {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoins);
                }
                isMoving = false;

            }
        }
        return newPosition - transform.position;

    }
    private void OnDrawGizmos()
    {
        if(localWaypoints.Length != 0)
        {
            Gizmos.color = Color.red;
            float size = .3f;

            for(int i = 0; i < localWaypoints.Length; ++i)
            {
                Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoins[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos + Vector3.left * size / 2, globalWaypointPos + Vector3.right * size / 2);
                Gizmos.DrawLine(globalWaypointPos + Vector3.up * size / 2, globalWaypointPos + Vector3.down * size / 2);

            }

        }
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Player"))
        {
            //collision.gameObject.transform.SetParent(transform);
            //groundPlayerController = collision.gameObject.GetComponent<GroundPlayerController>();
            collision.gameObject.transform.parent = transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.transform.parent = null;
            //collision.gameObject.transform.SetParent(transform);
            //groundPlayerController.SetPlatformXVelocity(0f);
        }
    }

    public void LerpTo(Transform position)
    {
        isTriggered = false;
        isMoving = true;
        percentBetweenWaypoints = 0;
        unTriggeredPosition = transform.position;
        triggeredPosition = position.position;
    }
}