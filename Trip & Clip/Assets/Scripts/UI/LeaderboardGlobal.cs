using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardGlobal : Leaderboard
{
   

    protected override void SetEntryValues(Transform entryTransform, User user)
    {
        base.SetEntryValues(entryTransform, user);
        entryTransform.Find("GlobalTime").GetComponent<Text>().text = user.globalTime.ToString();
        entryTransform.Find("Level1").GetComponent<Text>().text = user.levelsData[0].timeString;
        entryTransform.Find("Level2").GetComponent<Text>().text = user.levelsData[1].timeString;
    }

    protected override void SortUsers(List<User> usersList)
    {
       
        usersList.Sort((a, b) => a.globalTime.CompareTo(b.globalTime));
    }

}
