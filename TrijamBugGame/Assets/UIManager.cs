using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private int currentWater;
    private int maxWater;

    public TMP_Text waterAmtText;

    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
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
    }

    private void UpdateUI()
    {
        waterAmtText.text = "x " + currentWater.ToString() + " / " + maxWater.ToString();
    }
}
