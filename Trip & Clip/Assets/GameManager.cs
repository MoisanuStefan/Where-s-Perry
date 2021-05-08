using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    static GameManager gameManagerSingleton;
    // Start is called before the first frame update
    void Start()
    {
        if (gameManagerSingleton != null)
        {
            Object.Destroy(gameObject);
            return;
        }
        gameManagerSingleton = this;
        GameObject.DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
