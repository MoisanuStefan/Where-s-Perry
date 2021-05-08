using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used as a relay to call state trigger functions from foreign game objects. Great because this script can be attached to any game object that needs to call a Trigger/FinishAttack funtion from any type of AttackState class 
public class AnimationToStateMachine : MonoBehaviour
{
    public AttackState attackState;
   private void TriggerAttack()
    {
        attackState.TriggerAttack();
    }

    private void FinishAttack()
    {
        attackState.FinishAttack();
    }
}
