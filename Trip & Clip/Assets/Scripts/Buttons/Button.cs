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
    private bool collisionEnter = false;
    private bool collisionExit = false;

    private string triggeringEntity;

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
        
        if (collision.gameObject.GetComponent<PlayerController>().IsFocused() && !isTriggered)
        {
            triggeringEntity = collision.gameObject.tag;
            TriggerFunction();
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (isTriggered && collision.gameObject.CompareTag(triggeringEntity))
        {
            triggeringEntity = "";
            collisionExit = true;
            unTriggerTime = Time.time;
        }
        
        

    }



    // Update is called once per frame
    void Update()
    {
     
        if(collisionExit && Time.time >= unTriggerTime + delayTime)
        {
            collisionExit = false;
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
        
        yield return null;
    }

  
}
