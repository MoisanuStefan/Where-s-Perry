using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxController : MonoBehaviour
{
    [SerializeField]
    private float displayTime = 2f;
    private float enableTime;
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (gameObject.activeSelf && Time.time >= enableTime + displayTime)
        {
            text.text = "";
            gameObject.SetActive(false);
        }
    }
    public void SetMessage(string message)
    {
        text.text = message;
        gameObject.SetActive(true);
        enableTime = Time.time;
    }
}
