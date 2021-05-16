using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerSwitcher : MonoBehaviour
{
    static PlayerSwitcher playerSwitcherSingleton;


    public PlayerController[] players;
    private int currentPlayer;
    private bool isFollowEnabled = true;


    private void Start()
    {
        if (playerSwitcherSingleton != null)
        {
            Destroy(gameObject);
            return;
        }
        playerSwitcherSingleton = this;
        GameObject.DontDestroyOnLoad(gameObject);
        currentPlayer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        //vcam = camera.GetComponent<CinemachineVirtualCamera>();
       

            /*
            if (currentPlayer == 0)
            {

                //players[currentPlayer].GetComponentInChildren<GrapplingGun>().enabled = false;
                //material.friction = 0.4f;
                //players[currentPlayer].GetComponent<BoxCollider2D>().sharedMaterial = material;




                //players[1 - currentPlayer].GetComponent<Rigidbody2D>().isKinematic = false;

            }
            else
            {
                //players[currentPlayer].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                players[currentPlayer].GetComponent<FlyPlayerController>().SetFocused(false);

                //players[currentPlayer].GetComponent<Rigidbody2D>().isKinematic = true;


                players[1 - currentPlayer].GetComponent<PlayerController>().SetFocused(true);
                //players[1 - currentPlayer].GetComponentInChildren<GrapplingGun>().enabled = true;
                //material.friction = 0.0f;
                //players[1 - currentPlayer].GetComponent<BoxCollider2D>().sharedMaterial = material;

            }
           
            currentPlayer = 1 - currentPlayer;
            */

        
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
