using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private GameManager gm;
    private float currentHealth;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.transform.parent = null;
        GameObject.DontDestroyOnLoad(gameObject);
        gm.ResetScene();
    }
}
