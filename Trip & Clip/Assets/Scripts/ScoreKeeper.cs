using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class ScoreKeeper : MonoBehaviour
{
    private static ScoreKeeper instance;
    

    private int numberOfHats;


    private float elapsedTime;
    private bool timerStarted;

    private TimeSpan timePlaying;
    private TextMeshProUGUI hatsCounter;
    private TextMeshProUGUI timeCounter;
    private void Awake()
    {
        if (instance != null)
        {
            

            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetScore();
    }
    private void Start()
    {
        ResetScore();
    }

    public void ResetScore()
    {
        hatsCounter = GameObject.FindGameObjectWithTag("HatCounter").GetComponent<TextMeshProUGUI>();
        timeCounter = GameObject.FindGameObjectWithTag("TimeCounter").GetComponent<TextMeshProUGUI>();

        timeCounter.text = "Time: 00:00.00";
        timerStarted = false;
    }
    public static ScoreKeeper GetInstance()
    {
        return instance;
    }
    public void BeginTimer()
    {
        timerStarted = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerStarted = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (timerStarted)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
            timeCounter.text = timePlayingStr;
            yield return null;
        }
    }
    public void IncrementScore()
    {
        Debug.Log("incrementScore");
        numberOfHats += 1;
        hatsCounter.text = ": " + numberOfHats.ToString();
    }


}
