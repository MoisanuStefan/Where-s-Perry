using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LeaderboardLevel : Leaderboard
{
    [SerializeField]
    private int levelIndex;

    protected override void SetEntryValues(Transform entryTransform, User user)
    {
        base.SetEntryValues(entryTransform, user);
        entryTransform.Find("Time").GetComponent<TextMeshProUGUI>().text = (user.levelsData[levelIndex].timeString == "") ? "--:--.--" : user.levelsData[levelIndex].timeString;
    }

    protected override void SortUsers(List<User> usersList)
    {
        usersList.Sort((a, b) => a.levelsData[levelIndex].seconds.CompareTo(b.levelsData[levelIndex].seconds));

    }
}
