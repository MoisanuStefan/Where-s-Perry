using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelData
{
    public int id;
    public string name;
    public string timeString;
    public float seconds;

    public LevelData()
    {
        id = -1;
        name = "";
        timeString = "";
        seconds = 0;
    }

    public LevelData(int id, string name, string timeString, float seconds)
    {
        this.id = id;
        this.name = name;
        this.timeString = timeString;
        this.seconds = seconds;
    }
}
