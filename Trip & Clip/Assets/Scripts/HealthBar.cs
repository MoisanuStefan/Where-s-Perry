using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private static HealthBar singleton;
    [SerializeField]
    private GameObject[] hearts;
    [SerializeField]
    private Sprite fullHeart;
    [SerializeField]
    private Sprite emptyHeart;
    private int maxHealth = 2;
    private int currentHealth;

    private void Awake()
    {
        if (singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
        
    }
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public static HealthBar GetInstance()
    {
        return singleton;
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        foreach(GameObject heart in hearts)
        {
            heart.GetComponent<SpriteRenderer>().sprite = fullHeart;
        }
    }

    public void DecreaseHealth()
    {
        hearts[currentHealth].GetComponent<SpriteRenderer>().sprite = emptyHeart;
        currentHealth--;
    }

   
}
