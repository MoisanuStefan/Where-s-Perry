using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustParticle : MonoBehaviour
{
    private void AnimationFinished()
    {
        Object.Destroy(gameObject);
    }
}
