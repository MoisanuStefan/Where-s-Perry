using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlatform : MonoBehaviour
{

    public float speed;
    private float percentBetweenWaypoints;
    private bool isMoving;
    private Vector3 destination;

   


    private void FixedUpdate()
    {
        if (isMoving)
        {
            transform.Translate(CalculatePlatformMovement());
        }


    }



    private Vector3 CalculatePlatformMovement()
    {
        Vector3 newPosition;
        float distance = Vector3.Distance(transform.position, destination);
        percentBetweenWaypoints += Time.deltaTime * speed / distance;

        newPosition = Vector3.Lerp(transform.position, destination, percentBetweenWaypoints);
        if (1 - percentBetweenWaypoints < 0.1f)
        {
            percentBetweenWaypoints = 0;
            isMoving = false;
            newPosition = destination;

        }
        return newPosition - transform.position;

    }

    public void CancelMovement()
    {
        isMoving = false;
    }

    public bool IsMoving()
    {
        return isMoving;
    }
    public void LerpTo(Transform position)
    {
        destination = position.position;
        isMoving = true;
        percentBetweenWaypoints = 0;
    }
}
