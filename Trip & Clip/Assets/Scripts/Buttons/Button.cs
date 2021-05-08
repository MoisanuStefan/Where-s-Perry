using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Trigger
{
    public float moveDistance;
    public float lerpTime;
    public float delayTime = 0;

    public Trigger trigger;

    private bool isTriggered;
    private bool isMoving;
    private bool triggerEnter = false;
    private bool triggerExit = false;

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
        
        if (collision.gameObject.GetComponent<PlayerController>().IsFocused() && isMoving == false)
        {
            isMoving = true;
            triggerEnter = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>().IsFocused() && isMoving == false)
        {
            triggerExit = true;
            unTriggerTime = Time.time;
        }
        

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
                
                TriggerFunction();
            }
            triggerEnter = false;

        }

        else if(triggerExit && Time.time >= unTriggerTime + delayTime)
        {
            isMoving = true;
            triggerExit = false;
            TriggerFunction();
        }
      

    }

    public override void TriggerFunction()
    {
        base.TriggerFunction();
        Vector3 destination;
        trigger.TriggerFunction();

        destination = (isTriggered) ? unTriggeredPosition : triggeredPosition;

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
