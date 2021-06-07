using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Trigger
{
    public float moveDistance;
    public float lerpTime = 4f;
    public float delayTime = 0.1f;

    public Trigger[] triggers;

    private bool isTriggered;
    private bool isMoving;
    private bool collisionExit = false;
    private bool isDisabled = false;

    private string triggeringEntity = "notag";

    private float unTriggerTime;
    private float disabledTime = 0.4f;
    private float disableStartTime;


    private Vector3 unTriggeredPosition;
    private Vector3 triggeredPosition;



    // Start is called before the first frame update
    void Start()
    {
       unTriggeredPosition = transform.position;
       triggeredPosition = transform.position - transform.up.normalized * moveDistance;
        triggeringEntity = "notag";

       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (!isMoving)
        {
            if (triggeringEntity == "notag" || go.CompareTag(triggeringEntity))
            {
                // player layer = 10
                // pushables layer = 18
                if ((go.layer == 10 && go.GetComponent<PlayerController>().IsFocused()) || go.layer == 18)
                {
                    triggeringEntity = collision.gameObject.tag;
                    if (!isTriggered)
                    {

                        TriggerFunction();
                    }

                    else
                    {
                        unTriggerTime = Mathf.Infinity;
                    }
                }
            }
        }
            
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (!isMoving && isTriggered && collision.gameObject.CompareTag(triggeringEntity))
        {
            triggeringEntity = "notag";
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

    private void TriggerAll()
    {
        foreach (Trigger trigger in triggers)
        {
            trigger.TriggerFunction();
        }
    }
    public override void TriggerFunction()
    {
        base.TriggerFunction();
        Vector3 destination;
        TriggerAll();

        destination = (isTriggered) ? unTriggeredPosition : triggeredPosition;

        FindObjectOfType<SoundManager>().Play((isTriggered) ? "button_off" : "button_on");
        isTriggered = !isTriggered;
        StartCoroutine(Move(0f, destination));
    }

    IEnumerator Move(float perc, Vector3 destination)
    {
        float incrementAmount = 1f / lerpTime;
        
        while (1 - perc > 0.001f)
        {
            perc += incrementAmount;
            transform.position = Vector3.Lerp(transform.position, destination, perc);
            yield return new WaitForSeconds(0.005f);

        }
       
            isMoving = false;
            transform.position = destination;
        

    }

  
}
