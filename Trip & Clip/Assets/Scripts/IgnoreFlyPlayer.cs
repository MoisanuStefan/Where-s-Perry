using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreFlyPlayer : MonoBehaviour
{
    [SerializeField]
    private float collisionCheckRadius;
    [SerializeField]
    private LayerMask whoCantMoveMe;

    private float initMass;
    
    private Collider2D incoming;
    private Rigidbody2D rb;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initMass = rb.mass;

    }

    private void Update()
    {
        CheckCollisionIncoming();
    }

    private void CheckCollisionIncoming()
    {
        incoming = Physics2D.OverlapCircle(transform.position, collisionCheckRadius, whoCantMoveMe);
        if (incoming)
        {
            
             if (incoming.gameObject.CompareTag("FlyPlayer"))
            {
                rb.mass = 1000000;
            }
            else
            {
                rb.mass = initMass;
            }

        }
        else
        {
            rb.mass = initMass;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, collisionCheckRadius);
    }

    /*
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FlyPlayer"))
        {
            rb.mass = initMass;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FlyPlayer"))
        {
            rb.mass = 1000000;
        }
    }
    */
}
