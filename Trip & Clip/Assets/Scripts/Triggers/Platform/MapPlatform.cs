using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlatform : MonoBehaviour
{

    public float speed;
    public UnityEngine.UI.Button playButton;
    private float percentBetweenWaypoints;
    private bool isMoving;
    private Vector3 destination;
    private bool firstMoveInit = false;
    private bool firstMoveCompleted = false;
    private bool mustdisablePlayBut = false;



    private void Start()
    {
        firstMoveInit = false;
        firstMoveCompleted = false;
        mustdisablePlayBut = true;
    }

    private void OnEnable()
    {
        firstMoveInit = false;
        firstMoveCompleted = false;
        mustdisablePlayBut = true;
    }

    private void FixedUpdate()
    {
        if (mustdisablePlayBut)
        {
            mustdisablePlayBut = false;
            playButton.interactable = false;
        }
        if (isMoving)
        {
            transform.Translate(CalculatePlatformMovement());
        }
        if (!isMoving)
        {
            if (firstMoveInit && !firstMoveCompleted)
            {
                firstMoveCompleted = true;
                playButton.interactable = true;
                
            }
            FindObjectOfType<SoundManager>().Stop("platform_go");
            FindObjectOfType<SoundManager>().Stop("platform_come");



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
    public void LerpTo(Transform destination)
    {
        bool goingUp = transform.position.y <= destination.position.y;
        if (!isMoving && destination.position != transform.position)
        {
            FindObjectOfType<SoundManager>().Play("platform_init");
        }
        FindObjectOfType<SoundManager>().Stop((goingUp) ? "platform_come" : "platform_go");
        FindObjectOfType<SoundManager>().PlayLoop((goingUp) ? "platform_go" : "platform_come");
        this.destination = destination.position;
        isMoving = true;
        if (!firstMoveCompleted)
        {
            firstMoveInit = true;
        }
        percentBetweenWaypoints = 0;
    }
}
