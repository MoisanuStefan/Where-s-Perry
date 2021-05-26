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
    public float globalTime;
    public float currentLevel;
    public LevelData[] levelsData;


    public User(string localId, string email, string username, float globalTime)
    {
        levelsData = new LevelData[3];
        this.localId = localId;
        this.email = email;
        this.username = username;
        this.globalTime = globalTime;
        this.currentLevel = 0;
        
    }
}
