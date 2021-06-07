using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GroundPlayerController : PlayerController
{
    private static GroundPlayerController groundSingleton;


    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;

    
    

    private float platformXVelocity;

    private bool isGrounded;
    private bool canJump;

    public int amountOfJumps = 1;

    private int amountOfJumpsLeft;

    public LayerMask whatIsGround;
    public Transform groundCheck;
    public Transform followTarget;
    public GameObject flyPlayer;

    [SerializeField]
    private HealthBar healthBar;

    private void Awake()
    {
        if (groundSingleton != null)
        {
            Destroy(gameObject);
            return;
        }
        groundSingleton = this;
       
        GameObject.DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentHealth = maxHealth;
        isFocused = true;
        sceneLoaded = true;
        if (groundSingleton)
        {
            groundSingleton.transform.position = GameObject.FindGameObjectWithTag("ResetPosition").transform.position + Vector3.right * 0.8f;
            Physics2D.IgnoreCollision(groundSingleton.GetComponent<CapsuleCollider2D>(), FlyPlayerController.GetInstance().GetComponent<BoxCollider2D>());
        }
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
        else if(col.gameObject.CompareTag("FlyPlayerCollider") && groundCheck.position.y < (flyPlayer.transform.position.y + flyPlayer.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2) - 0.1)
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

   

    private void UpdateAnimations()
    {
        animator.SetBool("isWalking", Mathf.Abs(rb.velocity.x) > 0.001);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", (rb.velocity.y > 0) ? 1 : -1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hat"))
        {
            GameObject.FindGameObjectWithTag("ScoreKeeper").SendMessage("IncrementScore");
            Destroy(collision.gameObject);
        }
    }

    public override void SetFocused(bool value)
    {
        base.SetFocused(value);
    }

    public override void DecreaseHealth(float amount)
    {
        base.DecreaseHealth(amount);
        FindObjectOfType<SoundManager>().Play("grr");
        HealthBar.GetInstance().DecreaseHealth();

    }
    
    public override void Die()
    {
        HealthBar.GetInstance().ResetHealth();
        base.Die();
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(followTarget.position + Vector3.left * 0.1f, followTarget.position + Vector3.right * 0.1f);
        Gizmos.DrawLine(followTarget.position + Vector3.up * 0.1f, followTarget.position + Vector3.down * 0.1f);


        
    }
}
