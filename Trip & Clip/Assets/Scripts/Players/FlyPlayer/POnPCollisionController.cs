using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POnPCollisionController : MonoBehaviour
{
    public GameObject bottomThurster;
    public Transform playerCheck;
    public LayerMask whatIsPlayer;
    public float playerCheckRadius;

    private bool isPlayerOnHead = false;
    private bool playerIncoming = false;
    private bool isEnabled = true;
    private Vector3 positionBeforeImpact;

    private BoxCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        positionBeforeImpact = FlyPlayerController.GetInstance().transform.position;
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void Update()
    {
        CheckForPlayerIncoming();

    }

    public bool IsPlayerOnHead()
    {
        return isPlayerOnHead;
    }
    public void ResetPositionBeforeImpact()
    {
        positionBeforeImpact = FlyPlayerController.GetInstance().transform.position;
    }
    public void SetEnabled(bool value)
    {
        isEnabled = value;
        collider.enabled = value;
    }
    private void CheckForPlayerIncoming()
    {
        playerIncoming = Physics2D.OverlapCircle(playerCheck.position, playerCheckRadius, whatIsPlayer);
        if (playerIncoming)
        {
            positionBeforeImpact = FlyPlayerController.GetInstance().transform.position;
        }
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (GroundPlayerController.GetInstance().GetComponent<Rigidbody2D>().velocity.y < 0 && isEnabled && GroundPlayerController.GetInstance().groundCheck.position.y - (FlyPlayerController.GetInstance().transform.position.y + FlyPlayerController.GetInstance().GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2) > 0.01)
        {
            isPlayerOnHead = true;
            FlyPlayerController.GetInstance().GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            FlyPlayerController.GetInstance().GetComponent<Rigidbody2D>().isKinematic = true;
            FlyPlayerController.GetInstance().transform.Translate(positionBeforeImpact - transform.position);
            FlyPlayerController.GetInstance().GetComponent<FlyPlayerController>().DeactivateMovement();
            FlyPlayerController.GetInstance().GetComponent<FollowController>().DeactivateMovement();
        }
       
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isPlayerOnHead && isEnabled)
        {
            isPlayerOnHead = false;
            FlyPlayerController.GetInstance().GetComponent<Rigidbody2D>().isKinematic = false;
            FlyPlayerController.GetInstance().GetComponent<FlyPlayerController>().ActivateMovementWithDelay();
            FlyPlayerController.GetInstance().GetComponent<FollowController>().ActivateMovementWithDelay();

            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerCheck.position, playerCheckRadius);
    }
}
