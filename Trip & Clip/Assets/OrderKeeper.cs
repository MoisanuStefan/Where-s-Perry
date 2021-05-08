using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderKeeper : MonoBehaviour
{
    [SerializeField]
    private int[] order;
    [SerializeField]
    private Lever[] levers;
    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Notify(int id)
    { 
        

       if (index >= order.Length)
        {
            index = 0;
            ResetLevers();
        }
        else
        {
            if (order[index] != id)
            {
                index = 0;
                ResetLevers();
            }
            else
            {
                index++;
            }
        }

     
      
    }

    private void ResetLevers(int dontResetId = -1)
    {
        foreach (Lever lever in levers)
        {
            if (lever.id != dontResetId)
            {
                lever.Reset();
            }
            
        }
    }
}
