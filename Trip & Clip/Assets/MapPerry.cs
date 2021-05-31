﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPerry : MonoBehaviour
{
    public float movementSpeed;
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
        GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed, 0);
        anim.SetBool("isMoving", true);
    }
}
