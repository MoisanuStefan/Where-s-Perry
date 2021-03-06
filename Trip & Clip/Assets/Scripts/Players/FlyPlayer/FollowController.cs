using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

public class FollowController : MonoBehaviour
{
  
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float followDistance = 10f;
    public float headjumpMovementDelayTime = 0.5f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private bool isEnabled = true;
    private bool isFacingRight = true;
    private bool canMove = true;
    private float canMoveTime;

    private Seeker seeker;
    private Rigidbody2D rb;
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            // if current scene is not the menu, create path
            InvokeRepeating("UpdatePath", 0f, 0.2f);
        }
        

        
    }

    public void ResumePathBuilding()
    {
        InvokeRepeating("UpdatePath", 0f, 0.2f);

    }
    public void CancelPathBuilding()
    {
        CancelInvoke();
    }
    private void UpdatePath()
    {
        seeker.StartPath(rb.position, GameObject.FindGameObjectWithTag("FollowTarget").transform.position, OnPathComplete);

    }
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void SetEnabled(bool value)
    {
        isEnabled = value;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckIfCanMove();

        if (canMove)
        {
            ApplyMovement();
        }
        CheckFlip();
    }

    private void CheckIfCanMove()
    {
        if (!canMove && Time.time > canMoveTime + headjumpMovementDelayTime)
        {
            canMove = true;
        }
    }
    private void ApplyMovement()
    {
        if (path == null || !isEnabled)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count || Vector2.Distance(rb.position, GameObject.FindGameObjectWithTag("FollowTarget").transform.position) < followDistance)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        if (!reachedEndOfPath)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed;
            if (direction.x > 0) { }

            rb.AddForce(force);
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
    }


    public void DeactivateMovement()
    {
        canMove = false;
    }
    public void ActivateMovementWithDelay()
    {
        canMoveTime = Time.time;
        canMove = false;
    }
    private void CheckFlip()
    {
        if (isFacingRight && rb.velocity.x < 0 || !isFacingRight && rb.velocity.x > 0)
        {
            GetComponent<PlayerController>().Flip();
            isFacingRight = !isFacingRight;
        }
      
    }
   
}
