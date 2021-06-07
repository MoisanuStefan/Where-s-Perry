using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    protected bool isFocused;
    protected bool isFacingRight = true;
    protected bool canFlip = true;
    protected bool isWalking = false;
    protected bool canGetDamage = true;
    protected bool sceneLoaded = false;
    protected bool knockback;


    protected float horizontalMovementDirection = 0;
    protected float previousHorizontalDirection = 0;
    protected float previousVerticalDirection = 0;
    protected float currentHealth;
    protected float knockbackStartTime;

    [SerializeField]
    protected float maxHealth;
    [SerializeField]
    protected float knockbackDuration;


    protected Rigidbody2D rb;
    protected Animator animator;

    [SerializeField]
    protected Vector2 knockbackSpeed;
    [SerializeField]
    protected float spriteScaleFactor;

    public virtual void Start()
    {
        currentHealth = maxHealth;
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
        CheckKnockbackDone();


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
        if (value)
        {
            StartCoroutine(VisualFeedback());
        }
    }

    private IEnumerator VisualFeedback()
    {
        int loopTop = 20;
        int loopCounter = 0;
        Vector3 scaleIncrement = new Vector3(spriteScaleFactor, spriteScaleFactor, 0f);
        Vector3 initialScale = transform.localScale;
        while(loopCounter < loopTop)
        {
            transform.localScale += scaleIncrement;
            if (loopCounter == loopTop / 2)
            {
                scaleIncrement = -scaleIncrement;
            }
            loopCounter++;
            yield return new WaitForSeconds(0.001f);
        }
        transform.localScale = initialScale;
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
        if (canFlip && !knockback)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    public virtual void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        SetVelocity(Vector3.zero);
        gameObject.transform.parent = null;
        GameObject.DontDestroyOnLoad(gameObject);
        ManageGame.ResetScene();
    }

    public void Damage(AttackDetails attackDetails)
    {
        if (CanGetDamage())
        {
            int direction;
            DecreaseHealth(attackDetails.attackAmount);
            if (attackDetails.position.x < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            Knockback(direction);
        }
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

}
