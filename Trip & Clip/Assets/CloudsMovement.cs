using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsMovement : MonoBehaviour
{
    public float windSpeed = 3f;
    private float startPos, length;
    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    
    void Update()
    {
        transform.position = new Vector3(transform.position.x + windSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        if (transform.position.x - startPosition.x >= length)
        {
            transform.position = startPosition;
        }
    }
}
