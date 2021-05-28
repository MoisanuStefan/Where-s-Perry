using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTextPrint : MonoBehaviour
{
    [SerializeField]
    TextPrinter textPrinter;
    [SerializeField]
    private string text;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            textPrinter.WriteText(text);
        }
    }
}
