using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapPerry : MonoBehaviour
{
    public MapPlatform platform;
    public float movementSpeed;
    public UnityEngine.UI.Button playButton;
    public UnityEngine.UI.Button backButton;
    public Animator anim;
    private bool isMoving = false;


    private void Start()
    {

        isMoving = false;
    }

    private void FixedUpdate()
    {

    }

    public void Move()
    {
        if (!platform.IsMoving())
        {
            playButton.interactable = false;
            backButton.interactable = false;
            FindObjectOfType<SoundManager>().Stop("theme");
            FindObjectOfType<SoundManager>().Play("dubiduba");
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed, 0);
            anim.SetBool("isMoving", true);
        }
    }
}
