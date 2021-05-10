using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    private GroundPlayerController playerController;
    private float currentHealth;


    private void Start()
    {

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<GroundPlayerController>();
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
        playerController.SetDontDestroyOnLoad();
        GameManager.ResetScene();
    }
}
