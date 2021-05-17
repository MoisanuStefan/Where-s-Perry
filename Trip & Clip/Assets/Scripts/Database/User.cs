using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class User
{
    public string localId;
    public string email;
    public string username;


    public User(string localId, string email, string username)
    {
        this.localId = localId;
        this.email = email;
        this.username = username;
        
    }
}
