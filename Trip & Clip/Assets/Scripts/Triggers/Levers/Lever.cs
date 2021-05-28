using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Trigger
{
    public int id;
    [SerializeField]
    protected Trigger[] triggers;
    [SerializeField]
    private GameObject onLever;
    [SerializeField]
    private GameObject offLever;

    protected bool isOn = false;
    protected virtual void Start()
    {
        GetComponent<SpriteRenderer>().sprite = offLever.GetComponent<SpriteRenderer>().sprite;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("FlyPlayer")) && collision.gameObject.GetComponent<PlayerController>().IsFocused())

        {

            if (isOn)
            {
                GetComponent<SpriteRenderer>().sprite = offLever.GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = onLever.GetComponent<SpriteRenderer>().sprite;
            }
            isOn = !isOn;


            TriggerAll();
        }
    }



    public virtual void Reset()
    {
        if (isOn)
        {
            TriggerAll();
        }
        isOn = false;
        GetComponent<SpriteRenderer>().sprite = offLever.GetComponent<SpriteRenderer>().sprite;
    }

    public virtual void TriggerAll()
    {
        foreach (Trigger trigger in triggers)
        {
            trigger.TriggerFunction();
        }
    }
}
