using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerSwitcher : MonoBehaviour
{


    public PlayerController[] players;
    private int currentPlayer;
    private bool isFollowEnabled = true;


    private void Start()
    {
       
        currentPlayer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
       

        
    }

   
    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            FlyPlayerController flyplayer = FlyPlayerController.GetInstance();
            GroundPlayerController groundPlayer = GroundPlayerController.GetInstance();
            if (currentPlayer == 0)
            {
                flyplayer.SetFocused(true);
                flyplayer.SetFollowMode(false);
                groundPlayer.SetFocused(false);
               
                isFollowEnabled = false;
            }
            else
            {
                groundPlayer.SetFocused(true);
                flyplayer.SetFocused(false);
            }
           
            currentPlayer = 1 - currentPlayer;
        }
            if (Input.GetKeyDown(KeyCode.F))
        {
            isFollowEnabled = !isFollowEnabled;
            FlyPlayerController.GetInstance().SetFollowMode(isFollowEnabled);
        }
    }

}
