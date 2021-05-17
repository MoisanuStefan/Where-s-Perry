using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Proyecto26;
using UnityEngine.UI;

public class FirebaseHandler: MonoBehaviour
{
    public InputField usernameInput;
    public InputField emailInput;
    public InputField passwordInput;

    private static FirebaseHandler singletonInstance = null;

    private LevelData existingLevelData = null;
    private bool getRequestFinished = false;

    private string databaseURL = "https://bachelors-d701b-default-rtdb.europe-west1.firebasedatabase.app";
    private string authKey = "AIzaSyAwMdqR7son34YMsSLqhLvsJVvcbzW3SL8";
    private string idToken;
    private string localId;
    private string username;

    private void Awake()
    {
        if (singletonInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        singletonInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        existingLevelData = null;
        getRequestFinished = false;
    }
   

    public static FirebaseHandler GetInstance()
    {
        return singletonInstance;
    }

    
  public void PutLevelScore(string userName, int levelId, string levelName, float seconds)
    {

        RestClient.Get<LevelData>(databaseURL + "/userData/" + localId + "/levelsData/" + levelId + ".json").Then(response =>
        {
            if (response.seconds > seconds)
            {
                //there is a new highscore => update database entry
                string timeString = TimeSpan.FromSeconds(seconds).ToString("mm':'ss'.'ff");
                response.seconds = seconds;
                RestClient.Put(databaseURL + "/" + userName + "/" + levelId + ".json", response).Catch(err =>
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
        RestClient.Put(databaseURL + "/userData/" + localId + "/levelsData/" + levelId + ".json", levelData).Catch(err =>
        {
            var error = err as RequestException;
            Debug.Log("Error creating level score: " + err.Message);
        });
    }
    private void GetLevelData(string userName, int levelId)
    {
        RestClient.Get<LevelData>(databaseURL + "/userData/" + localId + "/levelsData/" + levelId + ".json").Then(response =>
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


    public void SignUpButton()
    {
        SignUpUser(emailInput.text, usernameInput.text, passwordInput.text);
    }

    public void SignInButton()
    {
        SignInUser(emailInput.text, passwordInput.text);
    }
    private void PostToDatabase()
    {
        User user = new User(localId, emailInput.text, usernameInput.text);

        RestClient.Put(databaseURL + "/userData/" +  localId + ".json?", user);
    }
    private void SignUpUser(string email, string username, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignInResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=" + authKey, userData).Then(
            response =>
            {
                idToken = response.idToken;
                localId = response.localId;
               
                PostToDatabase();

            }).Catch(error =>
            {
                Debug.Log(error);
            });
    }

    private void SignInUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignInResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + authKey, userData).Then(
            response =>
            {
                Debug.Log("Successfully signed in");
                idToken = response.idToken;
                localId = response.localId;
 
            }).Catch(error =>
            {
                Debug.Log(error);
            });
    }
}
