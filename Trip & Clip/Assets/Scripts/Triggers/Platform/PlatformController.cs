using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : Trigger
{
    public Vector3[] localWaypoints;

    public float speed;

    public bool isTriggerable;
    public bool isFromStart = false;

    private Vector3[] globalWaypoins;

    private float percentBetweenWaypoints;

    private int fromWaypointIndex;

    private BoxCollider2D boxCollider;
    private Bounds bounds;
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
        boxCollider = GetComponent<BoxCollider2D>();
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
            UpdateGraph();
        }


    }

    public override void TriggerFunction()
    {
        if (isTriggerable)
        {
            base.TriggerFunction();
            if (!isMoving && !isFromStart)
            {
                FindObjectOfType<SoundManager>().Play("platform_init");
            }
            FindObjectOfType<SoundManager>().Stop((isTriggered) ? "platform_go" : "platform_come");
            FindObjectOfType<SoundManager>().PlayLoop((isTriggered) ? "platform_come" : "platform_go");
            isTriggered = !isTriggered;
            percentBetweenWaypoints = 0;
            isMoving = true;
            if (boxCollider)
            {
                bounds = boxCollider.bounds;
            }
        }
    }
    private Vector3 CalculatePlatformMovement()
    {
        Vector3 newPosition;
        float distance;

        // platform only moves when TriggerFunction() is called
        if (isTriggerable)
        {
            Vector3 destination = (isTriggered) ? triggeredPosition : unTriggeredPosition;
            distance = Vector3.Distance(transform.position, destination);
            percentBetweenWaypoints += Time.deltaTime * speed / distance;

            newPosition = Vector3.Lerp(transform.position, destination, percentBetweenWaypoints);
            if (1 - percentBetweenWaypoints < 0.1f) {
                percentBetweenWaypoints = 0;
                isMoving = false;
                FindObjectOfType<SoundManager>().Play("platform_init");
                FindObjectOfType<SoundManager>().Stop((isTriggered) ? "platform_go" : "platform_come");

            }

        }
        // platform moves all the time
        else
        {
            int toWaypointIndex = fromWaypointIndex + 1;
            distance = Vector3.Distance(transform.position, globalWaypoins[toWaypointIndex]);
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

    private void UpdateGraph()
    {
        if (AstarPath.active)
        {
            AstarPath.active.UpdateGraphs(boxCollider.bounds);
            AstarPath.active.UpdateGraphs(bounds);
            bounds = boxCollider.bounds;
        }
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
