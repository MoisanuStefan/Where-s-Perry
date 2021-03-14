using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;

    private float movementInputDirection;

    private bool isFacingRight = true;
    private bool isGrounded;
    private bool canJump;

    public int amountOfJumps = 2;

    private int amountOfJumpsLeft;

    public LayerMask whatIsGround;
    public Transform groundCheck;
    public Camera camera;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        CheckInput();
        CheckLookingDirection();
        CheckIfCanJump();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
    public void CheckLookingDirection()
    {
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
       
        if (isFacingRight && mousePos.x < transform.position.x )
        {
            
            Flip();
        }
        else if (!isFacingRight && mousePos.x > transform.position.x)
        {
            Flip();

        }
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0)
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

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        amountOfJumpsLeft--;
    }
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump") && canJump)
        {
            Jump();
        }
    }

    private void ApplyMovement()
    {
        rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
    }

    public void ApplyForce(Vector3 force)
    {
        rb.AddForce(force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
