using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    protected Transform entryContainer;
    [SerializeField]
    protected Transform entryTemplate;
    [SerializeField]
    protected float templateHeight = 30f;
    protected List<Transform> entryTansforms;
    [SerializeField]
    protected ScrollRect scroll;
    private GameObject dialogBox;
    private void Awake()
    {
        entryTansforms = new List<Transform>();

    }

    private void OnEnable()
    {
        dialogBox = GameObject.FindGameObjectWithTag("DialogBox");
        if (dialogBox)
        {
            dialogBox.SetActive(false);
        }
    }
    private void OnDisable()
    {
        if (dialogBox)
        {
            dialogBox.SetActive(true);
        }
    }



    public void PopulateLeaderboard()
    {

        FirebaseHandler.GetInstance().GetUsersList((usersList) =>
        {
            SortUsers(usersList);
            SendEmptyToEnd(usersList);
            InstantiateEntries(usersList);
            DisplayEntries();
            ScrollToTop();


        }
        );

    }

    protected void ScrollToTop()
    {
        scroll.verticalNormalizedPosition = 0.5f;
        Canvas.ForceUpdateCanvases();
    }

    protected virtual void SendEmptyToEnd(List<User> usersList)
    {

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

            entryTransform.Find("Position").GetComponent<TextMeshProUGUI>().text = entryIndex.ToString();
            entryTransform.Find("Username").GetComponent<TextMeshProUGUI>().text = user.username;

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
