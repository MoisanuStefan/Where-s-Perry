using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SignInOutButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonText;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject signInMenu;
    [SerializeField]
    private GameObject registerMenu;
    private bool isSignIn = true;
    void Start()
    {


        Set();

    }

    private void OnEnable()
    {

        Set();

    }

    public void Set()
    {
        if (FirebaseHandler.GetInstance())
        {
            if (FirebaseHandler.GetInstance().isLoggedIn())
            {
                buttonText.text = "Sign Out";
                isSignIn = false;
            }
            else
            {
                buttonText.text = "Sign In";
                isSignIn = true;
            }
        }
    }

    public void ButtonPress()
    {
        FindObjectOfType<SoundManager>().Play("button_click");

        if (isSignIn)
        {
            mainMenu.SetActive(false);
            registerMenu.SetActive(true);
            signInMenu.SetActive(true);
        }
        else
        {
            FirebaseHandler.GetInstance().SignOutButton();
            Set();
        }
    }

}
