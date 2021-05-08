using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public int id;
    [SerializeField]
    protected Trapdoor trapdoor;
    [SerializeField]
    private GameObject onLever;
    [SerializeField]
    private GameObject offLever;

    protected bool isOn = false;
    protected virtual void Start()
    {
        GetComponent<SpriteRenderer>().sprite = offLever.GetComponent<SpriteRenderer>().sprite;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("FlyPlayer")) && collision.gameObject.GetComponent<FlyPlayerController>().IsFocused())

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
           

            trapdoor.Trigger();
        }
    }



    public virtual void Reset()
    {
        Debug.Log(id);
        if (isOn)
        {
            trapdoor.Trigger();
        }
        isOn = false;
        GetComponent<SpriteRenderer>().sprite = offLever.GetComponent<SpriteRenderer>().sprite;
    }

}
