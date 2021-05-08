using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderLever : Lever
{
    [SerializeField]
    private OrderKeeper orderKeeper;

    protected override void Start()
    {
        base.Start();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        StartCoroutine(TriggerAndNotify());
    }

    public override void Reset()
    {
        base.Reset();
    }

    IEnumerator TriggerAndNotify()
    {
        yield return new WaitForSeconds(0.2f);
        orderKeeper.Notify(id);

    }
}
