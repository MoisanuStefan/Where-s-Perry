using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCollisionController : MonoBehaviour
{
    public Camera cam;
    public Transform firePoint;
    public Transform receiverPoint;
    public Transform parent;
    private PolygonCollider2D collider;
    private LineRenderer lineRenderer;
    private AttackDetails attackDetails;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        collider = GetComponent<PolygonCollider2D>();
        SetCollider();
    }

    private void SetCollider()
    {
        Vector2[] colliderPointsV2 = new Vector2[4];

        float halfLineWidth = lineRenderer.startWidth / 2f;
        Vector3 leftLinePoint = firePoint.position;
        Vector3 rightLinePoint = receiverPoint.position;
        leftLinePoint -= parent.position;
        rightLinePoint -= parent.position;
      
        leftLinePoint.y -= halfLineWidth;
        colliderPointsV2[0] = leftLinePoint;
        leftLinePoint.y += 2 * halfLineWidth;
        colliderPointsV2[1] = leftLinePoint;
        rightLinePoint.y += halfLineWidth;
        colliderPointsV2[2] = rightLinePoint;
        rightLinePoint.y -= 2 * halfLineWidth;
        colliderPointsV2[3] = rightLinePoint;
        collider.SetPath(0, colliderPointsV2);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (lineRenderer.enabled)
        {
            attackDetails.attackAmount = 10000;
            attackDetails.position = gameObject.transform.position;
            collision.gameObject.SendMessage("Damage", attackDetails);
        }
    }
    // Update is called once per frame
    void Update()
    {

        
        if (lineRenderer.enabled)
        {
            SetCollider();
        }
    }


}
