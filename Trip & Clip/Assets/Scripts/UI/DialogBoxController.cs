using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogBoxController : MonoBehaviour
{
    [SerializeField]
    private float displayTime = 2f;
    private float enableTime;
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (gameObject.activeSelf && Time.time >= enableTime + displayTime)
        {
            text.text = "";
        }
    }
    public void SetMessage(string message)
    {
        text.text = message;
        gameObject.SetActive(true);
        enableTime = Time.time;
    }
}
