using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] players;
    public FollowCamera camera;

    public PhysicsMaterial2D material;

    private int currentPlayer;


    private void Start()
    {
        currentPlayer = 0;
        Physics2D.IgnoreCollision(players[0].GetComponent<BoxCollider2D>(), players[1].GetComponent<BoxCollider2D>());
        players[1].GetComponent<FlyPlayerController>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //vcam = camera.GetComponent<CinemachineVirtualCamera>();
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            if (currentPlayer == 0)
            {
                players[currentPlayer].GetComponent<PlayerController>().enabled = false;
                //players[currentPlayer].GetComponentInChildren<GrapplingGun>().enabled = false;
                material.friction = 0.4f;
                players[currentPlayer].GetComponent<BoxCollider2D>().sharedMaterial = material;

                players[1 - currentPlayer].GetComponent<FlyPlayerController>().enabled = true;
                players[1 - currentPlayer].GetComponent<FollowController>().enabled = false;

                //players[1 - currentPlayer].GetComponent<Rigidbody2D>().isKinematic = false;

            }
            else
            {
                players[currentPlayer].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                players[currentPlayer].GetComponent<FlyPlayerController>().enabled = false;
                //players[currentPlayer].GetComponent<Rigidbody2D>().isKinematic = true;


                players[1 - currentPlayer].GetComponent<PlayerController>().enabled = true;
                //players[1 - currentPlayer].GetComponentInChildren<GrapplingGun>().enabled = true;
                material.friction = 0.0f;
                players[1 - currentPlayer].GetComponent<BoxCollider2D>().sharedMaterial = material;

            }
           
            currentPlayer = 1 - currentPlayer;

            camera.SetFollowObject(players[currentPlayer]);
        }
    }

}
