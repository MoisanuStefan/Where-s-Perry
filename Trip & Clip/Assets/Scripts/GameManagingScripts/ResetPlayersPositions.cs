using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetPlayersPositions : MonoBehaviour
{
    public GameObject startPlatform;
    public float inactiveTime = 1f;
  

    PlayerController groundPlayer = null;
    FlyPlayerController flyPlayer = null;

    private float startTime;

    private void Start()
    {
        startTime = Mathf.Infinity;
        startPlatform = GameObject.FindGameObjectWithTag("StartPlatform");
        groundPlayer = GroundPlayerController.GetInstance();
        flyPlayer = FlyPlayerController.GetInstance();
        ResetPlayersPosition();
    }
   
    
    // Update is called once per frame
    void Update()
    {

        CheckInactiveTime();
       
    }

  

    private void ResetPlayersPosition()
    {
        if (groundPlayer != null && flyPlayer != null)
        {
           // GameObject.FindGameObjectWithTag("FlyPlayerCollider").SendMessage("SetEnabled", false);

            //colliderActivated = false;
            //flyPlayer.SetPosition(transform.position + Vector3.left * spawnOffset - flyPlayer.transform.position);
            flyPlayer.SendMessage("ResetPositionBeforeImpact");
            //groundPlayer.SetPosition(transform.position + Vector3.right * spawnOffset - groundPlayer.transform.position);
            flyPlayer.SetCanGetDamage(true);
            groundPlayer.SetCanGetDamage(true);
           
            startPlatform.SendMessage("TriggerFunction");
            startTime = Time.time;
        }
    }
    private void CheckInactiveTime()
    {
        if(Time.time >= startTime + inactiveTime)
        {
            //colliderActivated = true;
            //GameObject.FindGameObjectWithTag("FlyPlayerCollider").SendMessage("SetEnabled", Strue);
            startTime = Mathf.Infinity;
            groundPlayer.SetFocused(true);
            flyPlayer.SetFocused(false);
            flyPlayer.SetFollowMode(true);
            GameObject.FindGameObjectWithTag("ScoreKeeper").SendMessage("BeginTimer");
            FindObjectOfType<SoundManager>().Play("perry");
            FindObjectOfType<SoundManager>().Play("theme");

        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
