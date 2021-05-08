using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    [Header("General Refernces:")]
    public GrapplingGun grapplingGun;
    public LineRenderer m_lineRenderer;

    [Header("General Settings:")]

    float moveTime = 0;

    public bool isGrappling = true;


    private void OnEnable()
    {
        moveTime = 0;
        m_lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        m_lineRenderer.enabled = false;
        isGrappling = false;
    }

    private void Update()
    {
        moveTime += Time.deltaTime;
        if (!isGrappling)
        {
            grapplingGun.Grapple();
            isGrappling = true;
        }
        DrawRope();
    }

    void DrawRope()
    {
        m_lineRenderer.SetPosition(0, grapplingGun.firePoint.position);
        m_lineRenderer.SetPosition(1, grapplingGun.grapplePoint);
    }

}
