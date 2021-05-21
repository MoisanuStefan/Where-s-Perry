using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    protected Transform entryContainer;
    [SerializeField]
    protected Transform entryTemplate;
    [SerializeField]
    protected float templateHeight = 30f;
    protected List<Transform> entryTansforms;
    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);
        entryTansforms = new List<Transform>();

    }

  

   

    public void PopulateLeaderboard()
    {
       
        FirebaseHandler.GetInstance().GetUsersList((usersList) => {
            SortUsers(usersList);

            InstantiateEntries(usersList);

            DisplayEntries();

        }
        );

    }

    protected virtual void SortUsers(List<User> usersList)
    {

    }


    protected void InstantiateEntries(List<User> usersList)
    {
        int entryIndex = 1;
        foreach (User user in usersList)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * entryIndex + templateHeight / 2);

            entryTransform.Find("Position").GetComponent<Text>().text = entryIndex.ToString();
            entryTransform.Find("Username").GetComponent<Text>().text = user.username;

            SetEntryValues(entryTransform, user);
            entryTansforms.Add(entryTransform);

            entryIndex++;

        }
    }

    protected virtual void SetEntryValues(Transform entryTransform, User user)
    {

    }
   
    protected void DisplayEntries()
    {
        
        foreach (var entry in entryTansforms)
        {
            entry.gameObject.SetActive(true);
        }
    }

    public void ClearLeaderboard()
    {
        if (entryTansforms != null)
        {
            foreach (var entry in entryTansforms)
            {
                Destroy(entry.gameObject);
            }
            entryTansforms.Clear();
        }
    }
}
