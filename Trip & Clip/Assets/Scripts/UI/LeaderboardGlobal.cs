using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LeaderboardGlobal : Leaderboard
{
   

    protected override void SetEntryValues(Transform entryTransform, User user)
    {
        base.SetEntryValues(entryTransform, user);
        entryTransform.Find("GlobalTime").GetComponent<TextMeshProUGUI>().text = user.globalTime.ToString();
        entryTransform.Find("Level1").GetComponent<TextMeshProUGUI>().text = (user.levelsData[0].timeString == "") ? "--:--.--" : user.levelsData[0].timeString;
        entryTransform.Find("Level2").GetComponent<TextMeshProUGUI>().text = (user.levelsData[1].timeString == "") ? "--:--.--" : user.levelsData[1].timeString;
        entryTransform.Find("Level3").GetComponent<TextMeshProUGUI>().text = (user.levelsData[2].timeString == "") ? "--:--.--" : user.levelsData[2].timeString;
        entryTransform.Find("Level4").GetComponent<TextMeshProUGUI>().text = (user.levelsData[3].timeString == "") ? "--:--.--" : user.levelsData[3].timeString;
        entryTransform.Find("Level5").GetComponent<TextMeshProUGUI>().text = (user.levelsData[4].timeString == "") ? "--:--.--" : user.levelsData[4].timeString;
        entryTransform.Find("Level6").GetComponent<TextMeshProUGUI>().text = (user.levelsData[5].timeString == "") ? "--:--.--" : user.levelsData[5].timeString;
    }

    protected override void SortUsers(List<User> usersList)
    {
       
        usersList.Sort((a, b) => a.globalTime.CompareTo(b.globalTime));
    }

}
