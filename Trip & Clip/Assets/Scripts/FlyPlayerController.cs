using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPlayerController : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public Collider2D player1Collider;


    private float horizontalMovementDirection;
    private float verticalMovementDirection;

    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        ApplyMovement();
    }

    void ApplyMovement()
    {
        //rb.velocity = new Vector2(movementSpeed * horizontalMovementDirection, movementSpeed * verticalMovementDirection);
        rb.AddForce(new Vector2(movementSpeed * horizontalMovementDirection, movementSpeed * verticalMovementDirection));
    }

    void CheckInput()
    {
        horizontalMovementDirection = Input.GetAxisRaw("Horizontal");
        verticalMovementDirection = Input.GetAxisRaw("Vertical");
        
    }
}
