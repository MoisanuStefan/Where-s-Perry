using FullSerializer;
using Proyecto26;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class FirebaseHandler : MonoBehaviour
{
  
    public float requestWaitTime = 1f;
   

    private static FirebaseHandler singletonInstance = null;

    private string databaseURL = "https://bachelors-d701b-default-rtdb.europe-west1.firebasedatabase.app";
    private string authKey = "AIzaSyAwMdqR7son34YMsSLqhLvsJVvcbzW3SL8";
    private string idToken;
    private string localId;
    private string username;
    private bool isLogged = false;

    private List<User> usersList;

    private fsSerializer serializer;


    private void Awake()
    {
        if (singletonInstance != null)
        {
            return;
        }
        singletonInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
       

        serializer = new fsSerializer();
        usersList = new List<User>();

    }


    public static FirebaseHandler GetInstance()
    {
        return singletonInstance;
    }

    public void SignUpButton()
    {
        if (ValidInputFields())
        {
            SignUpUser(GameObject.FindGameObjectWithTag("EmailInput").GetComponent<InputField>().text, GameObject.FindGameObjectWithTag("UserInput").GetComponent<InputField>().text, GameObject.FindGameObjectWithTag("PassInput").GetComponent<InputField>().text);
        }
    }

    private bool ValidInputFields()
    {
        if (GameObject.FindGameObjectWithTag("PassVerifyInput").GetComponent<InputField>().text.Length == 0 || GameObject.FindGameObjectWithTag("PassInput").GetComponent<InputField>().text.Length == 0 || GameObject.FindGameObjectWithTag("EmailInput").GetComponent<InputField>().text.Length == 0)
        {
            GameObject.FindGameObjectWithTag("DialogBox").GetComponent<DialogBoxController>().SetMessage("Please fill all required fields.");
            return false;
        }
        if (GameObject.FindGameObjectWithTag("PassInput").GetComponent<InputField>().text.Length < 6)
        {
            GameObject.FindGameObjectWithTag("DialogBox").GetComponent<DialogBoxController>().SetMessage("Password must be at least 6 characters.");
            return false;
        }
        if (GameObject.FindGameObjectWithTag("PassInput").GetComponent<InputField>().text != GameObject.FindGameObjectWithTag("PassVerifyInput").GetComponent<InputField>().text)
        {
            GameObject.FindGameObjectWithTag("DialogBox").GetComponent<DialogBoxController>().SetMessage("Passwords do not match!");
            return false;
        }
        if (!GameObject.FindGameObjectWithTag("EmailInput").GetComponent<InputField>().text.Contains("@"))
        {
            GameObject.FindGameObjectWithTag("DialogBox").GetComponent<DialogBoxController>().SetMessage("Please insert a valid email.");
            return false;
        }
        return true;
    }

    private void SignUpUser(string email, string username, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignInResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=" + singletonInstance.authKey, userData).Then(
            response =>
            {
                CreateNewUserEntry(response.localId, response.idToken);
                GameObject.FindGameObjectWithTag("DialogBox").GetComponent<DialogBoxController>().SetMessage("Account created. Go back and sign in to play !");

            }).Catch(err =>
            {
                var error = err as RequestException;
                if (error != null)
                {
                    GameObject.FindGameObjectWithTag("DialogBox").GetComponent<DialogBoxController>().SetMessage("Error creating account. Try again.");
                    Debug.Log(error);
                }
            });
    }

    private void CreateNewUserEntry(string localId, string idToken)
    {
        User user = new User(localId, GameObject.FindGameObjectWithTag("EmailInput").GetComponent<InputField>().text, GameObject.FindGameObjectWithTag("UserInput").GetComponent<InputField>().text, 0f);

        RestClient.Put(singletonInstance.databaseURL + "/userData/" + localId + ".json?auth=" + idToken, user);
    }

    public void SignInButton()
    {
        if (singletonInstance.idToken != null)
        {
            GameObject.FindGameObjectWithTag("DialogBox").GetComponent<DialogBoxController>().SetMessage("Already signed in. Please sign out before signing in again.");
        }
        else
        {
            SignInUser(GameObject.FindGameObjectWithTag("EmailInput").GetComponent<InputField>().text, GameObject.FindGameObjectWithTag("PassInput").GetComponent<InputField>().text);
        }
    }

    private void SignInUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignInResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + singletonInstance.authKey, userData).Then(
            response =>
            {
                singletonInstance.isLogged = true;
                Debug.Log("Successfully signed in");
                GameObject.FindGameObjectWithTag("DialogBox").GetComponent<DialogBoxController>().SetMessage("Successfully signed in!");
                singletonInstance.idToken = response.idToken;
                singletonInstance.localId = response.localId;
               
                SetUsername();

            }).Catch(err =>
            {
                var error = err as RequestException;
                if (error != null)
                {
                    GameObject.FindGameObjectWithTag("DialogBox").GetComponent<DialogBoxController>().SetMessage("Username or password incorrect. Try again");
                    Debug.Log(error.Message);
                }
            });
    }

    private void SetUsername()
    {
        RestClient.Get<User>(singletonInstance.databaseURL + "/userData/" + singletonInstance.localId + ".json").Then(response =>
        {
            singletonInstance.username = response.username;
        }).Catch(err =>
        {
            var error = err as RequestException;
            if (error != null)
            {
                Debug.Log(error.Message);
                //TODO : display error for not being able to retreive username

            }


        });
    }

    public void SignOutButton()
    {
        singletonInstance.isLogged = false;
        singletonInstance.idToken = null;
        singletonInstance.localId = null;
        GameObject.FindGameObjectWithTag("DialogBox").GetComponent<DialogBoxController>().SetMessage("Successfully signed out.");
       
    }

    public bool isLoggedIn()
    {
        return singletonInstance.isLogged;
    }

    public void PutLevelScore(int levelId, string levelName, float seconds)
    {
        if (singletonInstance.idToken != null)
        {
            // idToken == nul => player is not signed in so scores will not get saved
            // idToken != null => player is signed in and the score will be updated if it is a highscore

            RestClient.Get<User>(singletonInstance.databaseURL + "/userData/" + singletonInstance.localId + ".json").Then(response =>
            {
                float previousLevelTime = response.levelsData[levelId].seconds;
                if (previousLevelTime > seconds || previousLevelTime == 0)
                {
                    //there is a new highscore => update database entries
                    string timeString = TimeSpan.FromSeconds(seconds).ToString("mm':'ss'.'ff");
                    response.globalTime = response.globalTime - previousLevelTime + seconds;
                    response.levelsData[levelId].timeString = timeString;
                    response.levelsData[levelId].seconds = seconds;
                    response.levelsData[levelId].id = SceneManager.GetActiveScene().buildIndex;
                    response.levelsData[levelId].name = SceneManager.GetActiveScene().name;
                    if (response.currentLevel == levelId)
                    {
                        // store the level with the lowest index that the user did not complete yet
                        response.currentLevel = levelId + 1;
                    }
                    RestClient.Put(singletonInstance.databaseURL + "/userData/" + singletonInstance.localId + ".json?auth=" + singletonInstance.idToken, response).Catch(err =>
                    {
                        var error = err as RequestException;
                        if (error != null)
                        {
                            Debug.Log("Error updating level score: " + err.Message);
                        }
                    });

                }
                else
                {
                    Debug.Log("not a highscore");
                }
            }).Catch(err =>
            {
                var error = err as RequestException;
                if (error != null)
                {

                    Debug.Log(err.Message);
                    Debug.Log(err.Data);
                }

                //TODO : display error for not being able to retreive data from database
            });
        }
    }

    public void GetUsersList(Action<List<User>> callback)
    {
        RestClient.Get(singletonInstance.databaseURL + "/userData.json").Then(response =>
        {

            fsData userDataJson = fsJsonParser.Parse(response.Text);
            Dictionary<string, User> users = null;
            singletonInstance.serializer.TryDeserialize(userDataJson, ref users);
            singletonInstance.usersList.Clear();
            foreach (var user in users.Values)
            {
                singletonInstance.usersList.Add(user);
            }
            callback(singletonInstance.usersList);
        }).Catch(err =>
        {

            var error = err as RequestException;
            if (error != null)
            {

                Debug.Log(err.Message);
                //TODO : display error for not being able to retreive users

            }
            

        });
    }

    public void GetCurrentLevel(Action<float> callback)
    {
        RestClient.Get<User>(singletonInstance.databaseURL + "/userData/" + singletonInstance.localId + ".json").Then(response =>
        {
            callback(response.currentLevel);
                
        
        }).Catch(err =>
        {
            var error = err as RequestException;
            if (error != null)
            {

                Debug.Log(err.Message);
            }

            //TODO : display error for not being able to retreive data from database
        });
    }

    public void PrintFeedbackReminder()
    {
        GetCurrentLevel((currentLevel) =>
        {
            if (currentLevel == 6)
            {
                
                GameObject.FindGameObjectWithTag("DialogBox").GetComponent<DialogBoxController>().SetTimer(20f);
                GameObject.FindGameObjectWithTag("DialogBox").GetComponent<DialogBoxController>().SetMessage("Thank you for playing! I hope you enjoyed it and please don't forget to fill out the feedback form. It only takes 5 minutes!");
            }
        });
    }

}
