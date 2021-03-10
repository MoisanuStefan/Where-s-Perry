using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public float lineWidth = 0.1f;
    public float speed = 75f;
    public float pullForce = 50f;


    public PlayerController playerController;
    public Material mat;
    public Rigidbody2D origin;
    
    private LineRenderer line;
    private Vector3 velocity;

    private bool isPulling = false;

    // Start is called before the first frame update
    void Start()
    {
        
        line = GetComponent<LineRenderer>();
        if (!line)
        {
            line = gameObject.AddComponent<LineRenderer>();
        }
        line.material = mat;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        isPulling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPulling)
        {
            Vector2 pullDirection = (Vector2)transform.position - origin.position;
            //pullDirection = pullDirection.normalized;
            playerController.ApplyForce(pullDirection * pullForce);
        }
        else
        {
            transform.position += velocity * Time.deltaTime;
        }
        
        line.SetPosition(0, transform.position);
        line.SetPosition(1, origin.position);
    }

    private void FixedUpdate()
    {
       
    }

    public void SetStart(Vector2 targetPositon)
    {
        Vector2 direction = targetPositon - origin.position;
        direction = direction.normalized;
        velocity = direction * speed;
        transform.position = origin.position;
        isPulling = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        velocity = Vector2.zero;
        isPulling = true;
    }
}
