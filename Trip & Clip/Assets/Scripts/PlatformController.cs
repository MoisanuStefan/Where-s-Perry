using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Vector3[] localWaypoints;

    public float speed;

    private Vector3[] globalWaypoins;

    private float percentBetweenWaypoints;

    private int fromWaypointIndex;
    private bool playerOn = false;

    private Rigidbody2D rb;
    private Transform playerTransform;
    private Vector3 platformDirection;
    private float platformMovement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fromWaypointIndex = 0;
        globalWaypoins = new Vector3[localWaypoints.Length];
        for(int i = 0; i < globalWaypoins.Length; ++i)
        {
            globalWaypoins[i] = localWaypoints[i] + transform.position;
        }
    }

    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, globalWaypoins[fromWaypointIndex + 1], 0.5f), speed * Time.deltaTime / Vector3.Distance(transform.position, globalWaypoins[fromWaypointIndex + 1]));
        Vector2 newPosition = CalculatePlatformMovement();
        //transform.position = Vector3.MoveTowards(transform.position, globalWaypoins[(fromWaypointIndex + 1) % globalWaypoins.Length], 0.5f);
        if (playerOn)
        {
           
           
        }
        transform.Translate(newPosition);
        

    }

    private void UpdatePlayerPosition(Vector3 newPlatformPosition)
    {
        Vector3 newPlayerPosition = playerTransform.position;
        
    }

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
    private Vector3 CalculatePlatformMovement()
    {
        int toWaypointIndex = fromWaypointIndex + 1;
        float distance = Vector3.Distance(transform.position, globalWaypoins[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distance;

        Vector3 newPosition = Vector3.Lerp(transform.position, globalWaypoins[toWaypointIndex], percentBetweenWaypoints);

        if (1 - percentBetweenWaypoints < 0.01f)       
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;
            if (fromWaypointIndex >= globalWaypoins.Length - 1)
            {
                fromWaypointIndex = 0;
                System.Array.Reverse(globalWaypoins);
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
            playerOn = true;
            playerTransform = collision.gameObject.transform;
            collision.gameObject.transform.parent = transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.transform.parent = null;
            playerOn = false;
        }
    }
}
