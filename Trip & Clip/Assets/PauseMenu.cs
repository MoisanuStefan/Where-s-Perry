using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PauseMenu : MonoBehaviour
{

    public Animator anim;
    public TextMeshProUGUI time;
    public TextMeshProUGUI hats;
    private bool isPaused = false;

    void Start()
    {
        time = GameObject.FindGameObjectWithTag("PauseTime").GetComponent<TextMeshProUGUI>();
        hats = GameObject.FindGameObjectWithTag("PauseHats").GetComponent<TextMeshProUGUI>();
        isPaused = false;
        anim = GetComponent<Animator>();   
    }

    public void ShowHideMenu()
    {
        time.text = GameObject.FindGameObjectWithTag("TimeCounter").GetComponent<TextMeshProUGUI>().text;
        hats.text = GameObject.FindGameObjectWithTag("HatCounter").GetComponent<TextMeshProUGUI>().text;
        if (!isPaused)
        {
            anim.SetTrigger("enter");
        }
        else
        {

            anim.SetTrigger("exit");

        }
        isPaused = !isPaused;
    }

  

   
}
