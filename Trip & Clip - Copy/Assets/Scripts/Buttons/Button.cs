using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public float moveDistance;
    public float lerpTime;
    public float delayTime = 0;

    public Trapdoor trapdoor;

    private bool isTriggered;
    private bool isColliding;
    private bool isMoving;
    private bool isDisabled;
    private bool delayTimerStarted = false;
    private bool triggerEnter = false;
    private bool triggerExit = false;

    private float disableTime = 0.2f;
    private float elapsedDisabledTime = 0f;
    private float elapsedTriggerDelay= 0f;
    private float unTriggerTime;

    private Vector3 unTriggeredPosition;
    private Vector3 triggeredPosition;



    // Start is called before the first frame update
    void Start()
    {
       unTriggeredPosition = transform.position;
       triggeredPosition = transform.position - transform.up.normalized * moveDistance;
      

       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("FlyPlayer") || collision.gameObject.GetComponent<FlyPlayerController>().IsFocused())
        {
            
            isColliding = true;
            triggerEnter = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggerExit = true;
        isColliding = false;
        unTriggerTime = Time.time;
        delayTimerStarted = true;

    }



    // Update is called once per frame
    void Update()
    {
      
        if (triggerEnter)
        {
            if(Time.time < unTriggerTime + delayTime)
            {
                unTriggerTime = Time.time;
            }
            else
            {
                
                Trigger();
            }
            triggerEnter = false;

        }

        if(triggerExit && Time.time >= unTriggerTime + delayTime)
        {
            triggerExit = false;
            Trigger();
        }
       
        /*
        if (isDisabled)
        {
            elapsedDisabledTime += Time.deltaTime;
            if (disableTime - elapsedDisabledTime <= 0)
            {
                isDisabled = false;
                elapsedDisabledTime = 0;
            }
        }


        if (!isDisabled && delayTimerStarted && Time.time >= unTriggerTime + delayTime)
        {
            delayTimerStarted = false;
            if (isColliding && !isTriggered)
            {
                isDisabled = true;

                Trigger();
            }

        }
        */

    }

    void Trigger()
    {
        Vector3 destination;
        trapdoor.Trigger();

        destination = (isTriggered) ? unTriggeredPosition : triggeredPosition;

        isMoving = true;
        isTriggered = !isTriggered;
        StartCoroutine(Move(0f, destination));
    }

    IEnumerator Move(float perc, Vector3 destination)
    {
        float incrementAmount = 1f / lerpTime;
        if (isMoving)
        {
            while (perc < 1f)
            {
                perc += Time.deltaTime * incrementAmount;
                transform.position = Vector3.Lerp(transform.position, destination, perc);
            }
            if (perc >= 1f)
            {
                isMoving = false;
                transform.position = destination;
            }
        }
        yield return null;
    }

  
}
