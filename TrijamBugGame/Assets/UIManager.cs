using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    private int currentWater;
    private int maxWater;

    private float currentHealth;
    private float maxHealth;

    public TMP_Text waterAmtText;
    public TMP_Text healthAmtText;

    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        UpdateValues();
        UpdateUI();
    }

    private void UpdateValues()
    {
        currentWater = playerMovement.amtOfWater;
        maxWater = playerMovement.maxWaterToHold;
        currentHealth = Mathf.Round(playerHealth.currentHealth);
        maxHealth = Mathf.Round(playerHealth.maxHealth);
    }

    private void UpdateUI()
    {
        waterAmtText.text = "x " + currentWater.ToString() + " / " + maxWater.ToString();
        // Round to whole number
        
        healthAmtText.text = "x " + currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
