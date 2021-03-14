using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] players;
    public GameObject camera;

    public PhysicsMaterial2D material;

    private int currentPlayer = 0;

    private CinemachineVirtualCamera vcam;

    private void Start()
    {
        vcam = camera.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        //vcam = camera.GetComponent<CinemachineVirtualCamera>();
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            players[currentPlayer].GetComponent<PlayerController>().enabled = false;
            players[currentPlayer].GetComponentInChildren<GrapplingGun>().enabled = false;
            material.friction = 0.4f;
            players[currentPlayer].GetComponent<CapsuleCollider2D>().sharedMaterial = material;
            


            players[1 - currentPlayer].GetComponent<PlayerController>().enabled = true;
            players[1 - currentPlayer].GetComponentInChildren<GrapplingGun>().enabled = true;
            material.friction = 0f;
            players[1 - currentPlayer].GetComponent<CapsuleCollider2D>().sharedMaterial = material;


            currentPlayer = 1 - currentPlayer;
            
            vcam.Follow = players[currentPlayer].transform;
        }
    }
}
