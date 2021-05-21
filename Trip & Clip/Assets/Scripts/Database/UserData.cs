using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class UserData 
{
    public string email;
    public string password;
    public bool returnSecureToken;

    public UserData(string email, string password)
    {
        this.email = email;
        this.password = password;
        returnSecureToken = true;
    }
}
