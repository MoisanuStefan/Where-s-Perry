using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject followObject;
    public float followOffset;
    public float switchSpeed = 20f;
    private Rigidbody2D rb;
    private float treshold;
    private bool isSwitching = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = followObject.GetComponent<Rigidbody2D>();
        treshold = ComputeTreshold();
    }

    public void SetFollowObject(GameObject followObject)
    {
        isSwitching = true;
        this.followObject = followObject;
        rb = followObject.GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 followPosition = followObject.transform.position;
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * followPosition.x);

        if (isSwitching && xDifference <= treshold)
        {
            isSwitching = false;
        }

        Vector3 newPosition = transform.position;
        if (Mathf.Abs(xDifference) >= treshold)
        {
            newPosition.x = followPosition.x;
        }
        float moveSpeed = isSwitching ? switchSpeed : rb.velocity.magnitude;
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.fixedDeltaTime);
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
