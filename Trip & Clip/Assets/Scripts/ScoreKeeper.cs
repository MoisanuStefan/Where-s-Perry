using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance;
    public TextMeshProUGUI text;
    private int numberOfHats;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    public void IncrementScore()
    {
        numberOfHats += 1;
        text.text = ": " + numberOfHats.ToString();
    }


}
