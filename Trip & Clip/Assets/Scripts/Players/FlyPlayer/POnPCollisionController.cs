using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POnPCollisionController : MonoBehaviour
{
    public GameObject flyPlayer;
    public GameObject player;
    public Transform playerCheck;
    public LayerMask whatIsPlayer;
    public float playerCheckRadius;

    private bool playerIncoming = false;
    private Vector3 positionBeforeImpact;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckForPlayerIncoming();
    }

    private void CheckForPlayerIncoming()
    {
        playerIncoming = Physics2D.OverlapCircle(playerCheck.position, playerCheckRadius, whatIsPlayer);
        if (playerIncoming)
        {
            positionBeforeImpact = flyPlayer.transform.position;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (player.GetComponent<Rigidbody2D>().velocity.y < 0)
        {

            flyPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            flyPlayer.GetComponent<Rigidbody2D>().isKinematic = true;
            flyPlayer.transform.Translate(positionBeforeImpact - transform.position);
        }
       
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        flyPlayer.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerCheck.position, playerCheckRadius);
    }
}
