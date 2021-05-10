using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPlayerController : PlayerController
{
    private static FlyPlayerController flySingleton;
    public POnPCollisionController popCollisioncontroller;
    public float movementSpeed = 10.0f;
    public float headjumpMovementDelayTime = 0.5f;
    public Collider2D player1Collider;
    public GameObject[] thrustParticles;

    private float verticalMovementDirection = 0;
    private float canMoveTime;
    private bool canMove = true;
    private bool isFollowing = true;
    private bool hasPlayerOn = false;

    private void Awake()
    {
        if (flySingleton != null)
        {
            Object.Destroy(gameObject);
            return;
        }
        flySingleton = this;
        GameObject.DontDestroyOnLoad(gameObject);
    }

    public override void Start()
    {
      
        base.Start();
        isFocused = false;
        
    }

    public static FlyPlayerController GetInstance()
    {
        return flySingleton;
    }
    public override void Update()
    {
        base.Update();
        UpdateThrustParticles();
    }

    public void ResetPositionBeforeImpact()
    {
        popCollisioncontroller.ResetPositionBeforeImpact();
    }
    public void SetFollowMode(bool value) {
        isFollowing = value;
        ResetThrusters();
        GetComponent<FollowController>().SetEnabled(value);
    }

    public override void FixedUpdate()
    {
        CheckIfCanMove();
        if (canMove)
        {
            base.FixedUpdate();
        }
        
    }
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();

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

   
    private void CheckIfCanMove()
    {
        if (!canMove && Time.time > canMoveTime + headjumpMovementDelayTime)
        {
            canMove = true;
        }
        hasPlayerOn = popCollisioncontroller.IsPlayerOnHead();
    }
  
   
    public override void ApplyMovement()
    {
        base.ApplyMovement();
        rb.AddForce(new Vector2(movementSpeed * horizontalMovementDirection, movementSpeed * verticalMovementDirection));
       
    }

    private void UpdateThrustParticles()
    {
        if (!hasPlayerOn && canMove)
        {
            if (!isFollowing && isFocused)
            {
                thrustParticles[0].SetActive(verticalMovementDirection == -1);
                thrustParticles[1].SetActive(horizontalMovementDirection != 0);
                thrustParticles[2].SetActive(verticalMovementDirection == 1);
            }
            else if (isFollowing)
            {
                thrustParticles[0].SetActive(rb.velocity.y < -0.01);
                thrustParticles[1].SetActive(rb.velocity.x < -0.01 || rb.velocity.x > 0.1);
                thrustParticles[2].SetActive(rb.velocity.y > 0.01);
            }
        }
        else if (hasPlayerOn)
        {
            thrustParticles[2].SetActive(true);
        }
        else
        {
            thrustParticles[2].SetActive(false);
        }
      
        
          
        
    }

    public bool HasPlayerOn()
    {
        return hasPlayerOn;
    }

    public void ResetThrusters()
    {
        foreach(GameObject go in thrustParticles)
        {
            go.SetActive(false);
        }
    }

    public override void SetFocused(bool value)
    {
        base.SetFocused(value);
        ResetThrusters();
    }

    public override void CheckInput()
    {
        base.CheckInput();
       
        previousVerticalDirection = verticalMovementDirection;
        verticalMovementDirection = Input.GetAxisRaw("Vertical");
        if (verticalMovementDirection != previousVerticalDirection || horizontalMovementDirection != previousHorizontalDirection)
        {
            if (isFollowing && isFocused)
            {
                SetFollowMode(false);
            }
        }
        
    }
}
