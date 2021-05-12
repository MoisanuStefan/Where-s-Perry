using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetPlayersPositions : MonoBehaviour
{
    private static ResetPlayersPositions instance;
    public GameObject startPlatform;
    public float inactiveTime = 1f;
  

    PlayerController groundPlayer = null;
    FlyPlayerController flyPlayer = null;

    private bool resetInitiated = false;
    private bool resetDone = false;
    private bool colliderActivated = false;
    private float startTime;

    private ScoreKeeper scoreKeeper;
    private GameObject flyPlayerCollider;


    private void Awake()
    {
        if (instance != null)
        {
            Object.Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        startPlatform = GameObject.FindGameObjectWithTag("StartPlatform");
        colliderActivated = false;
        resetInitiated = true;
        resetDone = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        flyPlayerCollider = GameObject.FindGameObjectWithTag("FlyPlayerCollider");


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
            flyPlayer.SetFollowMode(true);
            ScoreKeeper.GetInstance().BeginTimer();
        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
