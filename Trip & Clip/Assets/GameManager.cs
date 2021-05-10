using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    static GameManager gameManagerSingleton;

    private void Awake()
    {
        if (gameManagerSingleton != null)
        {
            Object.Destroy(gameObject);
            return;
        }
        gameManagerSingleton = this;
        GameObject.DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
       

    }

    public static GameManager GetInstance()
    {
        return gameManagerSingleton;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ResetScene()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
