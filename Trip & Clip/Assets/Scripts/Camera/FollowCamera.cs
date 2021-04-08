using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject followObject;
    public float followOffset;
    public float speed;
    private Rigidbody2D rb;
    private float treshold;

    // Start is called before the first frame update
    void Start()
    {
        rb = followObject.GetComponent<Rigidbody2D>();
        treshold = ComputeTreshold();
    }

    public void SetFollowObject(GameObject followObject)
    {
        this.followObject = followObject;
        rb = followObject.GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 followPosition = followObject.transform.position;
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * followPosition.x);

        Vector3 newPosition = transform.position;
        Debug.Log(Mathf.Abs(xDifference));
        if (Mathf.Abs(xDifference) >= treshold)
        {
            newPosition.x = followPosition.x;
        }
        float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }

    private float ComputeTreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        float treshold = Camera.main.orthographicSize * aspect.width / aspect.height;
        treshold -= followOffset;
        return treshold;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        float border = ComputeTreshold();
        Gizmos.DrawWireCube(transform.position, new Vector2(border * 2, Camera.main.orthographicSize * 2));
    }

}
