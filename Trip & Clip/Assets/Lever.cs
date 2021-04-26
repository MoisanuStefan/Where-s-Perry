using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField]
    private Trapdoor trapdoor;
    [SerializeField]
    private GameObject onLever;
    [SerializeField]
    private GameObject offLever;

    private bool isOn = false;
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = offLever.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

}
