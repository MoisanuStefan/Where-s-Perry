using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    List<TabButton> tabButtons;
    [SerializeField]
    private Color tabIdle;
    [SerializeField]
    private Color tabHover;
    [SerializeField]
    private Color tabActive;
    [SerializeField]
    private TabButton selectedTab;
    [SerializeField]
    private List<GameObject> pagesToSwap;

  

    private void OnEnable()
    {
        OnTabSelected(selectedTab);   
    }

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.color = tabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }
 
        selectedTab = button;

        selectedTab.Select();
        ResetTabs();
        button.background.color = tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < pagesToSwap.Count; ++i)
        {
            if (i == index)
            {
                pagesToSwap[i].SetActive(true);
            }
            else
            {
                pagesToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        if (tabButtons != null)
        {
            foreach (TabButton button in tabButtons)
            {
                if (selectedTab != null && button != selectedTab)
                {
                    button.background.color = tabIdle;
                }
            }
        }
    }

    public void DeselectSelectedTab()
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }
    }

    public void OnBackButtonClick()
    {
        selectedTab.Deselect();
        
    }

   
}
