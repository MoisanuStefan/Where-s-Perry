using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAStarGraph : MonoBehaviour
{
    private Collider2D aCollider;
    private Rigidbody2D rb;
    private Bounds bounds;
    void Start()
    {
       
        aCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            UpdateGraph();
        }
    }

    private void UpdateGraph()
    {
        if (AstarPath.active)
        {
            AstarPath.active.UpdateGraphs(aCollider.bounds);
            AstarPath.active.UpdateGraphs(bounds);
            bounds = aCollider.bounds;
        }
    }
}
