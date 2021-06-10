using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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

    private IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(1f);
        ResetScene();


    }
    public void RestartButton()
    {
        Time.timeScale = 1;
        StartCoroutine(DelaySceneLoad());

    }
    public void MenuButton()
    {
        Time.timeScale = 1;
        Destroy(FlyPlayerController.GetInstance().gameObject);
        Destroy(GroundPlayerController.GetInstance().gameObject);
        SceneManager.LoadScene("Menu");
    }
    public void PauseUnpauseGame()
    {
        scoreKeeper.PauseResumeTimer();
        isPaused = !isPaused;
        GameObject.FindGameObjectsWithTag("PauseButton")[1].GetComponentInChildren<TextMeshProUGUI>().text = (isPaused) ? "RESUME" : "PAUSE";
        if (!isPaused)
        {
            Time.timeScale = 1;

        }
        else
        {

            StartCoroutine(DelayPause());
        }
    }

    private IEnumerator DelayPause()
    {
        yield return new WaitForSeconds(0.5f);


        Time.timeScale = 0;
    }

    
}
