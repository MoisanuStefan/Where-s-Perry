using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;

    public D_Entity entityData;
    public int facingDirection { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject aliveGO { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }

    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;
    [SerializeField]
    private Transform playerCheck;

    private int lastDamageDirection;

    private Vector2 velocityWorkspace;  // holder for any vector, instead of creating a new vector each time

    private float currentHealth;

    protected bool isDead;
    public virtual void Start()
    {
        currentHealth = entityData.maxHealth;
        facingDirection = 1;
        aliveGO = transform.Find("Alive").gameObject;
        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent<Animator>();
        atsm = aliveGO.GetComponent<AnimationToStateMachine>();

        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        //return Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
        return Physics2D.OverlapBox(wallCheck.position, new Vector2(entityData.wallCheckDistance, entityData.wallCheckDistance * 2), 0, entityData.whatIsGround);
    }

    public virtual bool CheckLedge()
    {
        return !Physics2D.Raycast(ledgeCheck.position, Vector3.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }

    public bool CheckPlayerInMinRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.minDetectRange, entityData.whatIsPlayer);
    }

    public bool CheckPlayerInMaxRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.maxDetectRange, entityData.whatIsPlayer);

    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

   
    public virtual void Flip()
    {
        facingDirection *= -1;
        aliveGO.transform.Rotate(0f, 180f, 0f);
    }

    public virtual void DamageHop(float velocity)
    {
        velocityWorkspace.Set(rb.velocity.x, velocity);
        rb.velocity = velocityWorkspace;
    }
    public virtual void Damage(AttackDetails attackDetails)
    {
        currentHealth -= attackDetails.attackAmount;
        DamageHop(entityData.damageHopSpeed);
        if (attackDetails.position.x > aliveGO.transform.position.x)
        {
            lastDamageDirection = -1;
        }
        else
        {
            lastDamageDirection = 1;
        }

        if (currentHealth <= 0)
        {
            isDead = true;
        }

    }
    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(wallCheck.position, new Vector2(entityData.wallCheckDistance, entityData.wallCheckDistance * 2));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
        Gizmos.DrawWireSphere(playerCheck.position + playerCheck.right * entityData.minDetectRange, 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + playerCheck.right * entityData.maxDetectRange, 0.3f);


    }
}
