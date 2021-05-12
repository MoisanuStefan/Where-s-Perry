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

    private void Awake()
    {
        if (groundSingleton != null)
        {
            Object.Destroy(gameObject);
            return;
        }
        groundSingleton = this;
        GameObject.DontDestroyOnLoad(gameObject);
    }
    public override void Start()
    {
       
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
        Collider2D col;
        col = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (!col)
        {
            isGrounded = false;
        }
        // make sure jump is enabled only if player is on top of flyplayer, not just by colliding
        else if(col.gameObject.CompareTag("FlyPlayer") && groundCheck.position.y - (flyPlayer.transform.position.y + flyPlayer.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2) < 0.01)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = col;
        }
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
            Debug.Log("har");
            ScoreKeeper.GetInstance().IncrementScore();
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

    public void SetDontDestroyOnLoad()
    {
        gameObject.transform.parent = null;
        GameObject.DontDestroyOnLoad(gameObject);
    }

   
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(followTarget.position + Vector3.left * 0.1f, followTarget.position + Vector3.right * 0.1f);
        Gizmos.DrawLine(followTarget.position + Vector3.up * 0.1f, followTarget.position + Vector3.down * 0.1f);


        
    }
}
