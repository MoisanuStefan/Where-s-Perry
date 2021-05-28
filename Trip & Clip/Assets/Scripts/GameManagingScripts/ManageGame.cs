using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageGame : MonoBehaviour
{

    private bool isPaused = false;
    [SerializeField]
    private ScoreKeeper scoreKeeper;

    private void Start()
    {
        isPaused = false;
    }

   
    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {

        SceneManager.LoadScene("Level0");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public static void ResetScene()
    {
        Time.timeScale = 1;
        Debug.Log("Scene reset");
        GroundPlayerController.GetInstance().transform.parent = null;
        DontDestroyOnLoad(GroundPlayerController.GetInstance().gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartButton()
    {
        ResetScene();
    }
    public void MenuButton()
    {
        Destroy(FlyPlayerController.GetInstance().gameObject);
        Destroy(GroundPlayerController.GetInstance().gameObject);
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    public void PauseUnpauseGame()
    {
        isPaused = !isPaused;
        scoreKeeper.PauseResumeTimer();
        GameObject.FindGameObjectWithTag("PauseButton").GetComponentInChildren<Text>().text = (isPaused) ? "Resume" : "Pause";
        Time.timeScale = (isPaused) ? 0 : 1;
    }
}
