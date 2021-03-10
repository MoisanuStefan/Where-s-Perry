using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSetter : MonoBehaviour
{
    public HookController hook;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hook.SetStart(worldPosition);
        }
    }
}
