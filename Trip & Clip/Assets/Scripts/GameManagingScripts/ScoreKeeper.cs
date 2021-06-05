using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ScoreKeeper : MonoBehaviour
{
    private static ScoreKeeper instance;


    private int numberOfHats;
    private int shakeDirection;

    private float elapsedTime;
    private bool timerStarted;
    private bool isShaking = false;
    [SerializeField]
    private float shakeFactor = 0.5f;
    private float shakeBeginTime;
    private float shakeTime = 0.5f;
    [SerializeField]
    private float shakeSpeed;

    private TimeSpan timePlaying;
    private Text hatsCounter;
    private Text timeCounter;
    private Transform hatContainerTransform;
    private Vector3 initialPosition;
   

    
    private void Start()
    {
        ResetScore();
    }

  
    public void ResetScore()
    {
        hatsCounter = GameObject.FindGameObjectWithTag("HatCounter").GetComponent<Text>();
        timeCounter = GameObject.FindGameObjectWithTag("TimeCounter").GetComponent<Text>();
        numberOfHats = 0;

        timeCounter.text = "00:00.00";
        timerStarted = false;
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
        if (FirebaseHandler.GetInstance().isLoggedIn())
        {
            FirebaseHandler.GetInstance().PutLevelScore(SceneManager.GetActiveScene().buildIndex - 1, SceneManager.GetActiveScene().name, elapsedTime);
        }
        else
        {
            if (PlayerPrefs.GetInt("CurrentLevel") <= SceneManager.GetActiveScene().buildIndex - 1)
            {
                PlayerPrefs.SetInt("CurrentLevel", SceneManager.GetActiveScene().buildIndex);
            }
        }

    }

    public void PauseResumeTimer()
    {
        timerStarted = !timerStarted;
        if (timerStarted)
        {
            StartCoroutine(UpdateTimer());
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (timerStarted)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");
            timeCounter.text = timePlayingStr;
            yield return null;
        }
    }
    public void IncrementScore()
    {
        Debug.Log("incrementScore");
        numberOfHats += 1;
        hatsCounter.text = numberOfHats.ToString() + " / 3";
    }

    public bool AllHatsCollected()
    {
        return numberOfHats == 3;
    }
    public void ShakeHatsCounterContainer()
    {
        if (!isShaking)
        {
            isShaking = true;
            shakeBeginTime = Time.time;
            shakeDirection = -1;
            hatContainerTransform = GameObject.FindGameObjectWithTag("HatContainer").transform;
            initialPosition = hatContainerTransform.position;
            hatContainerTransform.position += Vector3.left * shakeFactor / 2;
            StartCoroutine(ShakeMe());
        }

    }

    private IEnumerator ShakeMe()
    {
        while (Time.time < shakeBeginTime + shakeTime)
        {
            hatContainerTransform.position += Vector3.left * shakeDirection * shakeFactor;
            shakeDirection *= -1;
            yield return new WaitForSeconds(shakeSpeed);
        }

        hatContainerTransform.position = initialPosition;
        isShaking = false;

    }
}
