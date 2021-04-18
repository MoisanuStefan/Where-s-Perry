using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newChargeStateData", menuName = "Data/State Data/Charge State Data")]

public class D_ChargeState : ScriptableObject
{
    public float chargeSpeed = 20f;
    public float chargeTime = 3f;
}
