using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer, attackRadius, attackDamage;
    [SerializeField]
    private Transform attackHitBoxPosition;
    [SerializeField]
    private LayerMask whatIsDamageable;

    private bool gotInput;
    private bool isAttacking;

    private AttackDetails attackDetails;

    private float lastInputTime = -Mathf.Infinity;

    private Animator animator;
    private GroundPlayerController playerController;
    private PlayerStats playerStats;


    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<GroundPlayerController>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }
    private void CheckCombatInput()
    {
        
        if (Input.GetMouseButtonDown(0) && playerController.IsFocused() && !EventSystem.current.IsPointerOverGameObject())
        {
            if (combatEnabled)
            {
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
        
    }

    private void CheckAttacks()
    {
        if (gotInput)
        {
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                animator.SetBool("isAttacking", isAttacking);
            }
        }
        if (Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackHitBoxPosition.position, attackRadius, whatIsDamageable);

        attackDetails.attackAmount = attackDamage;
        attackDetails.position = transform.position;
        foreach(Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attackDetails);
        }
    }

    private void FinishAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
    }

    public void Damage(AttackDetails attackDetails)
    {
        if (playerController.CanGetDamage())
        {
            int direction;
            playerStats.DecreaseHealth(attackDetails.attackAmount);
            if (attackDetails.position.x < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            playerController.Knockback(direction);
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackHitBoxPosition.position, attackRadius);
    }
}
