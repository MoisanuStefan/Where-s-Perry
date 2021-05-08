using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetPlayersPositions : MonoBehaviour
{
    public GameObject startPlatform;
    public float inactiveTime = 1f;
    public GameObject flyPlayerCollider;
    PlayerController groundPlayer = null;
    PlayerController flyPlayer = null;

    private bool resetInitiated = false;
    private bool resetDone = false;
    private bool colliderActivated = false;
    private float startTime;

    private void Awake()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameObject.DontDestroyOnLoad(gameObject);

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        startPlatform = GameObject.FindGameObjectWithTag("StartPlatform");
        resetInitiated = true;
        resetDone = false;
    }
    // Start is called before the first frame update
    void Start()
    {
       // done = false;
       


    }

    
    // Update is called once per frame
    void Update()
    {
        CheckInactiveTime();
        if (resetInitiated)
        {
            ResetPlayersPosition();
        }
    }

    private void ResetPlayersPosition()
    {
       
        
        if (!resetDone)
        {
            if (groundPlayer == null)
            {
                groundPlayer = GroundPlayerController.GetInstance();
            }
            if (flyPlayer == null)
            {
                flyPlayer = FlyPlayerController.GetInstance();

            }
            if (groundPlayer != null && flyPlayer != null)
            {

                flyPlayerCollider.SetActive(false);
                colliderActivated = false;
                flyPlayer.SetPosition(transform.position - flyPlayer.transform.position);
                flyPlayer.SendMessage("ResetPositionBeforeImpact");
                groundPlayer.SetPosition(transform.position - groundPlayer.transform.position);
                startPlatform.SendMessage("TriggerFunction");
                startTime = Time.time;
                resetDone = true;
                resetInitiated = false;
            }
        }
        
    }
    private void CheckInactiveTime()
    {
        if(!colliderActivated && Time.time >= startTime + inactiveTime)
        {
            colliderActivated = true;
            flyPlayerCollider.SetActive(true);
            groundPlayer.SetFocused(true);
        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
