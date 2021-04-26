﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;

    [SerializeField]
    private Vector2 knockbackSpeed;
    [SerializeField]
    private float knockbackDuration;

    private float movementInputDirection;
    private float knockbackStartTime;
    private float platformXVelocity;

    private bool isFacingRight = true;
    private bool isGrounded;
    private bool canJump;
    private bool isWalking = false;
    private bool canFlip = true;
    private bool isFollowing = true;
    private bool knockback;
    private bool isFocused = true;

    public int amountOfJumps = 1;

    private int amountOfJumpsLeft;

    public LayerMask whatIsGround;
    public Transform groundCheck;
    public Transform followTarget;
    public Camera camera;
    public Animator animator;
    public GameObject flyPlayer;
    public ScoreKeeper scoreKeeper;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        amountOfJumpsLeft = amountOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFocused)
        {
            CheckInput();
        }
        CheckIfCanJump();
        UpdateAnimations();
        CheckKnockbackDone();
    }

    private void FixedUpdate()
    {
        CheckMovementDirection();
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckInput()
    {
      
            movementInputDirection = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump") && canJump)
            {
                Jump();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                isFollowing = !isFollowing;
                flyPlayer.GetComponent<FollowController>().enabled = isFollowing;

            }
        
    }
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    public void SetFocused(bool value)
    {
        isFocused = value;
        movementInputDirection = 0;
    }

    public bool IsFocused()
    {
        return isFocused;
    }
    public void CheckMovementDirection()
    {
       
       
        if (canFlip && isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (canFlip && !isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.x) < 0.001)
        {
            isWalking = false;
            
        }
        else
        {
            isWalking = true;
        }
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.001)
        {
            amountOfJumpsLeft = amountOfJumps;
        }
        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    public void DisableFlip()
    {
        canFlip = false;
    }
    public void Flip()
    {
        if (canFlip && !knockback)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        amountOfJumpsLeft--;
    }
   

    private void ApplyMovement()
    {
        if (!knockback)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection + platformXVelocity, rb.velocity.y);
        }
    }

    public void SetPlatformXVelocity(float xVelocity)
    {
        platformXVelocity = xVelocity;

    }

    public void ApplyForce(Vector3 force)
    {
        rb.AddForce(force);
    }

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void CheckKnockbackDone()
    {
        if (knockback && Time.time >= knockbackStartTime + knockbackDuration)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    private void UpdateAnimations()
    {
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hat"))
        {
            scoreKeeper.IncrementScore();
            Destroy(collision.gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(followTarget.position + Vector3.left * 0.1f, followTarget.position + Vector3.right * 0.1f);
        Gizmos.DrawLine(followTarget.position + Vector3.up * 0.1f, followTarget.position + Vector3.down * 0.1f);

    }
}
