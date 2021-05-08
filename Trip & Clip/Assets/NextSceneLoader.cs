using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextSceneLoader : MonoBehaviour
{
    public string sceneName;
    public float waitTime;

    public Vector3 playersResetPosition;


    private bool groundPlayerOn;
    private bool flyPlayerOn;
    private GameObject flyPlayer;
    private GameObject groundPlayer;

  
  
    private void Start()
    {
        groundPlayerOn = false;
        flyPlayerOn = false;
    }

    private void Update()
    {
       
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            groundPlayerOn = true;
            groundPlayer = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("FlyPlayer"))
        {
            flyPlayerOn = true;
            flyPlayer = collision.gameObject;
        }

        if (groundPlayerOn && flyPlayerOn)
        {
            groundPlayer.SendMessage("SetFocused", false);
            flyPlayer.SendMessage("SetFocused", false);
            groundPlayer.transform.parent = null;
            GameObject.DontDestroyOnLoad(groundPlayer);
            GetComponentInParent<PlatformController>().TriggerFunction();
            StartCoroutine(LoadNextScene(sceneName));           
        }
    }

  
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            groundPlayerOn = false;
        }
        else if (collision.gameObject.CompareTag("FlyPlayer"))
        {
            flyPlayerOn = false;
        }
    }

    IEnumerator LoadNextScene(string sceneName) {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneName);

    }

   

}
