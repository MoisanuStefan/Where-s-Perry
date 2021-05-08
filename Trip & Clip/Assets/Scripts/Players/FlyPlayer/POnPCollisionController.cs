using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POnPCollisionController : MonoBehaviour
{
    public GameObject flyPlayer;
    public GameObject player;
    public GameObject bottomThurster;
    public Transform playerCheck;
    public LayerMask whatIsPlayer;
    public float playerCheckRadius;

    private bool isPlayerOnHead = false;
    private bool playerIncoming = false;
    private bool isEnabled = true;
    private Vector3 positionBeforeImpact;

    // Start is called before the first frame update
    void Start()
    {
        positionBeforeImpact = flyPlayer.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void Update()
    {
        CheckForPlayerIncoming();

    }

    public void ResetPositionBeforeImpact()
    {
        positionBeforeImpact = flyPlayer.transform.position;
    }
    public void SetEnabled(bool value)
    {
        isEnabled = value;
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
        
        if (player.GetComponent<Rigidbody2D>().velocity.y < 0 && isEnabled)
        {
            isPlayerOnHead = true;
            flyPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            flyPlayer.GetComponent<Rigidbody2D>().isKinematic = true;
            flyPlayer.transform.Translate(positionBeforeImpact - transform.position);
            flyPlayer.GetComponent<FlyPlayerController>().DeactivateMovement();
            flyPlayer.GetComponent<FollowController>().DeactivateMovement();
            bottomThurster.SetActive(true);
        }
       
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isPlayerOnHead && isEnabled)
        {
            isPlayerOnHead = false;
            flyPlayer.GetComponent<Rigidbody2D>().isKinematic = false;
            flyPlayer.GetComponent<FlyPlayerController>().ActivateMovementWithDelay();
            flyPlayer.GetComponent<FollowController>().ActivateMovementWithDelay();

            bottomThurster.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerCheck.position, playerCheckRadius);
    }
}
