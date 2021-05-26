using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPrinter : MonoBehaviour
{
    [SerializeField]
    private float pauseBetweenLetters = 0.1f;
    [SerializeField]
    private Text textGui;
    [SerializeField]
    Animator anim;
    private bool isPrinting = false;
    private bool requestWaiting = false;
    private string text;



    public void WriteText(string text)
    {
        requestWaiting = true;
        this.text = text;
        if (!isPrinting)
        {
            anim.SetBool("isWriting", true);
            requestWaiting = false;
            isPrinting = true;
            textGui.text = "";
            StartCoroutine(TypeText());
        }
    }

    private void Update()
    {
        if (requestWaiting && !isPrinting)
        {
            requestWaiting = false;
            isPrinting = true;
            textGui.text = "";
            StartCoroutine(TypeText());
        }
    }
    private IEnumerator TypeText()
    {
        foreach (char letter in text.ToCharArray())
        {
            if (letter == '\\')
            {
                textGui.text += '\n';
            }
            else
            {
                textGui.text += letter;
                yield return new WaitForSeconds(pauseBetweenLetters);
            }
        }
        isPrinting = false;
        anim.SetBool("isWriting", false);



    }

    

}
