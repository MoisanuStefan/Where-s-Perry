using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject -> scriptable container used to store large amounts of data independent of class instances
[CreateAssetMenu(fileName = "newMoveStateData", menuName = "Data/State Data/Move State")]
public class D_MoveState : ScriptableObject
{
    public float movementSpeed = 3f;
}
