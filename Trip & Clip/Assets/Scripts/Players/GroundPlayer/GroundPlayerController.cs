using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GroundPlayerController : PlayerController
{
    private static GroundPlayerController groundSingleton;


    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;

    [SerializeField]
    private Vector2 knockbackSpeed;
    [SerializeField]
    private float knockbackDuration;

    private float knockbackStartTime;
    private float platformXVelocity;

    private bool isGrounded;
    private bool canJump;
    private bool isFollowing = true;
    private bool knockback;

    public int amountOfJumps = 1;

    private int amountOfJumpsLeft;

    public LayerMask whatIsGround;
    public Transform groundCheck;
    public Transform followTarget;
    public Camera camera;
    public GameObject flyPlayer;
    public ScoreKeeper scoreKeeper;

    public override void Start()
    {
        if (groundSingleton != null)
        {
            Object.Destroy(gameObject);
            return;
        }
        groundSingleton = this;
        GameObject.DontDestroyOnLoad(gameObject);
        base.Start();
        isFocused = true;
        amountOfJumpsLeft = amountOfJumps;
        
    }

    public static GroundPlayerController GetInstance()
    {
        return groundSingleton;
    }

    public override void Update()
    {
        base.Update();
        CheckIfCanJump();
        UpdateAnimations();
        CheckKnockbackDone();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        CheckSurroundings();
    }

    public override void CheckInput()
    {
        base.CheckInput();
        if (Input.GetButtonDown("Jump") && canJump)
            {
                Jump();
            }
        /*
            if (Input.GetKeyDown(KeyCode.F))
            {
                isFollowing = !isFollowing;
                flyPlayer.GetComponent<FollowController>().enabled = isFollowing;

            }
        */
        
    }
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

   

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.001)
        {
            amountOfJumpsLeft = amountOfJumps;
        }
        if (amountOfJumpsLeft <= 0 || !isFocused)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

   

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        amountOfJumpsLeft--;
    }
   

    public override void ApplyMovement()
    {
        base.ApplyMovement();
        if (!knockback)
        {
            rb.velocity = new Vector2(movementSpeed * horizontalMovementDirection + platformXVelocity, rb.velocity.y);
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

    public override void Flip()
    {
        if (!knockback)
        {
            base.Flip();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(followTarget.position + Vector3.left * 0.1f, followTarget.position + Vector3.right * 0.1f);
        Gizmos.DrawLine(followTarget.position + Vector3.up * 0.1f, followTarget.position + Vector3.down * 0.1f);

    }
}
