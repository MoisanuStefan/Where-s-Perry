using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ManageGame : MonoBehaviour
{
    static ManageGame gameManagerSingleton;

    private void Awake()
    {
        if (gameManagerSingleton != null)
        {
            return;
        }
        gameManagerSingleton = this;
        GameObject.DontDestroyOnLoad(gameObject);
    }
    void Start()
    {


    }

    public static ManageGame GetInstance()
    {
        return gameManagerSingleton;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ResetScene()
    {
        Debug.Log("Scene reset");
        GroundPlayerController.GetInstance().transform.parent = null;
        DontDestroyOnLoad(GroundPlayerController.GetInstance().gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
