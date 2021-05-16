using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Proyecto26;

public class FirebaseHandler
{
    private static FirebaseHandler singletonInstance = null;

    private LevelData existingLevelData = null;
    private bool getRequestFinished = false;


    private FirebaseHandler()
    {
        existingLevelData = null;
        getRequestFinished = false;
    }

    public static FirebaseHandler GetInstance()
    {
        if (singletonInstance == null)
        {
            singletonInstance = new FirebaseHandler();
        }
        return singletonInstance;
    }

    
  public void PutLevelScore(string userName, int levelId, string levelName, float seconds)
    {

        RestClient.Get<LevelData>("https://bachelors-d701b-default-rtdb.europe-west1.firebasedatabase.app/" + userName + "/" + levelId + ".json").Then(response =>
        {
            if (response.seconds > seconds)
            {
                //there is a new highscore => update database entry
                string timeString = TimeSpan.FromSeconds(seconds).ToString("mm':'ss'.'ff");
                response.seconds = seconds;
                RestClient.Put("https://bachelors-d701b-default-rtdb.europe-west1.firebasedatabase.app/" + userName + "/" + levelId + ".json", response).Catch(err =>
                {
                    var error = err as RequestException;
                    Debug.Log("Error updating level score: " + err.Message);
                });
            }
            else
            {
                Debug.Log("not a highscore");
            }
        }).Catch(err =>
        {
            var error = err as RequestException;
            Debug.Log(err.Message);
            Debug.Log(err.Data);
            if (err.Message == "JSON must represent an object type.")
            {
                CreateNewLevelEntry(userName, levelId, levelName, seconds);
            }
            //TODO : display error for not being able to retreive data from database
        });
    }

    private void CreateNewLevelEntry(string userName, int levelId, string levelName, float seconds)
    {
        string timeString = TimeSpan.FromSeconds(seconds).ToString("mm':'ss'.'ff");
        LevelData levelData = new LevelData(levelId, levelName, timeString, seconds);
        RestClient.Put("https://bachelors-d701b-default-rtdb.europe-west1.firebasedatabase.app/" + userName + "/" + levelId + ".json", levelData).Catch(err =>
        {
            var error = err as RequestException;
            Debug.Log("Error updating level score: " + err.Message);
        });
    }
    private void GetLevelData(string userName, int levelId)
    {
        RestClient.Get<LevelData>("https://bachelors-d701b-default-rtdb.europe-west1.firebasedatabase.app/" + userName + "/" + levelId + ".json").Then(response =>
        {
            existingLevelData = response;
            getRequestFinished = true;
        }).Catch(err =>
        {
            getRequestFinished = true;
            var error = err as RequestException;
            Debug.Log(err.Message);
            //TODO : display error for not being able to retreive data from database
        });
      
    }
}
