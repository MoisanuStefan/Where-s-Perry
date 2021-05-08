using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : Trigger
{
    public bool isTriggered;

    public Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;
    public Transform receiver;

    public PolygonCollider2D lineCollider;
    private Vector3[] colliderPoints = new Vector3[2];

    // Start is called before the first frame update
    void Start()
    {
        isTriggered = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, receiver.position);
       
        EnableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            UpdateLaser();
        }
    }

    public override void TriggerFunction()
    {
        base.TriggerFunction();
        if (!isTriggered)
        {
            EnableLaser();
        }
        else
        {
            DisableLaser();
        }
        isTriggered = !isTriggered;
    }
    private void EnableLaser()
    {
        lineRenderer.enabled = true;
       
    }

    private void DisableLaser()
    {
        lineRenderer.enabled = false;
    }

    private void UpdateLaser()
    {
        lineRenderer.SetPosition(1, receiver.position);
    }

   

}
