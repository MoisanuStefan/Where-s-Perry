using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    protected Transform resetPosition;
    protected bool isFocused;
    protected bool isFacingRight = true;
    protected bool canFlip = true;
    protected bool isWalking = false;
    protected bool canGetDamage = true;
    protected bool sceneLoaded = false;



    protected float horizontalMovementDirection = 0;
    protected float previousHorizontalDirection = 0;
    protected float previousVerticalDirection = 0;

    protected Rigidbody2D rb;
    protected Animator animator;
    
    public virtual void Start()
    {
       
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetPosition(Vector3 position)
    {
        transform.Translate(position);
    }
   
    public virtual void Update()
    {
        CheckInput();
        CheckSceneLoaded();
       
    }

    public virtual void FixedUpdate()
    {
        if (isFocused)
        {
            CheckMovementDirection();
            ApplyMovement();
        }
    }

    public virtual void CheckSceneLoaded()
    {
        if (sceneLoaded)
        {
            sceneLoaded = false;
            rb.velocity = Vector3.zero;
        }
    }
    public virtual void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }
    public virtual void SetCanGetDamage(bool value)
    {
        canGetDamage = value;
    }

    public virtual bool CanGetDamage()
    {
        return canGetDamage;
    }
    public virtual void ApplyMovement()
    {

    }

    public void CheckMovementDirection()
    {


        if (canFlip && isFacingRight && horizontalMovementDirection < 0)
        {
            Flip();
        }
        else if (canFlip && !isFacingRight && horizontalMovementDirection > 0)
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



    public virtual void CheckInput()
    {
        
        previousHorizontalDirection = horizontalMovementDirection;
        horizontalMovementDirection = Input.GetAxisRaw("Horizontal");
       

    }

    public virtual void SetFocused(bool value)
    {
        isFocused = value;

        if (isFocused)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        rb.velocity = new Vector2(0f, rb.velocity.y);
        horizontalMovementDirection = 0;
    }

    public bool IsFocused()
    {
        return isFocused;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    public void DisableFlip()
    {
        canFlip = false;
    }
    public virtual void Flip()
    {
        if (canFlip)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

   
}
