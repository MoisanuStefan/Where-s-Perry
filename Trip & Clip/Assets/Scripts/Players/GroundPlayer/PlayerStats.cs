using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]

    private GroundPlayerController playerController;


    private void Start()
    {

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<GroundPlayerController>();
    }

  
}
