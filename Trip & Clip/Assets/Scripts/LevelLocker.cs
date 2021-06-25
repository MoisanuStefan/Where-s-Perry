using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class LevelLocker : MonoBehaviour
{
    [SerializeField]
    private GameObject perry;
    [SerializeField]
    private Transform level0Position;
    [SerializeField]
    private MapPlatform platform;
    [SerializeField]
    private Trapdoor[] trapdoors;
    [SerializeField]
    private GameObject[] buttons;

    private void Start()
    {
        CloseAll();
    }
    private void OnEnable()
    {
        StartCoroutine(DelayInintialization());
    }
    public void CloseAll()
    {
        foreach (Trapdoor trapdoor in trapdoors)
        {
            if (trapdoor.isTriggered)
            {
                trapdoor.TriggerFunction();
            }
        }
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }
        perry.transform.position = level0Position.position + Vector3.up;
        platform.transform.position = level0Position.position;
    }

    public void UnlockAvailableLevels()
    {
        if (FirebaseHandler.GetInstance().isLoggedIn())
        {
            FirebaseHandler.GetInstance().GetCurrentLevel((currentLevel) =>
            {
                int index = 1;
                foreach (GameObject button in buttons)
                {
                    if (index <= currentLevel)
                        button.SetActive(true);
                    index++;

                }
                StartCoroutine(UnlockAvailableLevelsEnum(currentLevel));


            });
        }
        else
        {
            int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            int index = 1;
            foreach (GameObject button in buttons)
            {
                if (index <= currentLevel)
                    button.SetActive(true);
                index++;

            }
            StartCoroutine(UnlockAvailableLevelsEnum(currentLevel));
        }
    }

    private IEnumerator DelayInintialization()
    {
        yield return new WaitForSeconds(1f);
        UnlockAvailableLevels();
    }

    public void CancelPlatformMovement()
    {
        platform.CancelMovement();
    }
    private IEnumerator UnlockAvailableLevelsEnum(float currentLevel)
    {
        int index = 1;
        foreach (Trapdoor trapdoor in trapdoors)
        {
            if (index <= currentLevel)
            {
                index++;
                trapdoor.TriggerFunction();
                yield return new WaitForSeconds(0.2f);
            }
        }
        if(currentLevel == 6)
        {
            currentLevel--;
        }
        if (currentLevel < 6)
        {
            platform.LerpTo(buttons[(int)currentLevel].transform);
        }
      
    }

  
}
