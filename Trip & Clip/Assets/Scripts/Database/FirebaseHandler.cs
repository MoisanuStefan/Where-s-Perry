using FullSerializer;
using Proyecto26;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirebaseHandler : MonoBehaviour
{
    [SerializeField]
    private DialogBoxController dialogBox;
    [SerializeField]
    private GameObject signOutButton;
    public float requestWaitTime = 1f;
    public InputField usernameInput;
    public InputField emailInput;
    public InputField passwordInput;
    public InputField passordCheckInput;

    private static FirebaseHandler singletonInstance = null;

    private string databaseURL = "https://bachelors-d701b-default-rtdb.europe-west1.firebasedatabase.app";
    private string authKey = "AIzaSyAwMdqR7son34YMsSLqhLvsJVvcbzW3SL8";
    private string idToken;
    private string localId;
    private string username;


    private List<User> usersList;

    private fsSerializer serializer;


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
            SignUpUser(emailInput.text, usernameInput.text, passwordInput.text);
        }
    }

    private bool ValidInputFields()
    {
        if (passordCheckInput.text.Length == 0 || passwordInput.text.Length == 0 || emailInput.text.Length == 0)
        {
            dialogBox.SetMessage("Please fill all required fields.");
            return false;
        }
        if (passwordInput.text.Length < 6)
        {
            dialogBox.SetMessage("Password must be at least 6 characters.");
            return false;
        }
        if (passwordInput.text != passordCheckInput.text)
        {
            dialogBox.SetMessage("Passwords do not match!");
            return false;
        }
        if (!emailInput.text.Contains("@"))
        {
            dialogBox.SetMessage("Please insert a valid email.");
            return false;
        }
        return true;
    }

    private void SignUpUser(string email, string username, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignInResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=" + authKey, userData).Then(
            response =>
            {
                CreateNewUserEntry(response.localId, response.idToken);
                dialogBox.SetMessage("Account created. Go back and sign in to play !");

            }).Catch(err =>
            {
                var error = err as RequestException;
                if (error != null)
                {
                    dialogBox.SetMessage("Error creating account. Try again.");
                    Debug.Log(error);
                }
            });
    }

    private void CreateNewUserEntry(string localId, string idToken)
    {
        User user = new User(localId, emailInput.text, usernameInput.text, 0f);

        RestClient.Put(databaseURL + "/userData/" + localId + ".json?auth=" + idToken, user);
    }

    public void SignInButton()
    {
        if (idToken != null)
        {
            dialogBox.SetMessage("Already signed in. Please sign out before signing in again.");
        }
        else
        {
            SignInUser(emailInput.text, passwordInput.text);
        }
    }

    private void SignInUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignInResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + authKey, userData).Then(
            response =>
            {
                Debug.Log("Successfully signed in");
                dialogBox.SetMessage("Successfully signed in!");
                idToken = response.idToken;
                localId = response.localId;
                signOutButton.SetActive(true);
                SetUsername();

            }).Catch(err =>
            {
                var error = err as RequestException;
                if (error != null)
                {
                    dialogBox.SetMessage("Username or password incorrect. Try again");
                    Debug.Log(error.Message);
                }
            });
    }

    private void SetUsername()
    {
        RestClient.Get<User>(databaseURL + "/userData/" + localId + ".json").Then(response =>
        {
            username = response.username;
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
        idToken = null;
        localId = null;
        dialogBox.SetMessage("Successfully signed out.");
    }


    public void PutLevelScore(int levelId, string levelName, float seconds)
    {
        if (idToken != null)
        {
            // idToken == nul => player is not signed in so scores will not get saved
            // idToken != null => player is signed in and the score will be updated if it is a highscore

            RestClient.Get<User>(databaseURL + "/userData/" + localId + ".json").Then(response =>
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
                    RestClient.Put(databaseURL + "/userData/" + localId + ".json?auth=" + idToken, response).Catch(err =>
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
        RestClient.Get(databaseURL + "/userData.json").Then(response =>
        {

            fsData userDataJson = fsJsonParser.Parse(response.Text);
            Dictionary<string, User> users = null;
            serializer.TryDeserialize(userDataJson, ref users);
            usersList.Clear();
            foreach (var user in users.Values)
            {
                usersList.Add(user);
            }
            callback(usersList);
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


}
