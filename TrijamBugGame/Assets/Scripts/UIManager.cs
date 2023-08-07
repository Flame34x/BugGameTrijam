using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Private Variables

    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    private int currentWater;
    private int maxWater;

    private float currentHealth;
    private float maxHealth;
    private float playerTimeAlive = 0f;

    private bool isPlayerAlive = true;

    #endregion

    #region Public Variables

    public TMP_Text waterAmtText;
    public TMP_Text currentTimeText;
    public TMP_Text highScoreTimeText;

    public TMP_Text currentGO;
    public TMP_Text highGO;

    public RawImage[] lifeImages;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        StartCoroutine(UpdateTimeAlive());
    }

    private void Update()
    {
        UpdateValues();
        UpdateUI();
    }

    #endregion

    #region Private Methods

    private void UpdateValues()
    {
        currentWater = playerMovement.amtOfWater;
        currentHealth = Mathf.Round(playerHealth.currentHealth);
        maxHealth = Mathf.Round(playerHealth.maxHealth);
    }

    private void UpdateHighScore()
    {
        // Retrieve the previous high score from PlayerPrefs
        float previousHighScore = PlayerPrefs.GetFloat("Highscore", 0f);

        // Check if the current player time alive is greater than the previous high score
        if (playerTimeAlive > previousHighScore)
        {
            // If it's greater, update the high score in PlayerPrefs
            PlayerPrefs.SetFloat("Highscore", playerTimeAlive);
            PlayerPrefs.Save();
        }

        // Display the high score on the UI
        highScoreTimeText.text = "High Score: " + FormatTime(previousHighScore);
    }

    private string FormatTime(float timeInSeconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    private void UpdateUI()
    {
        waterAmtText.text = "x" + currentWater.ToString();

        UpdateHighScore();
        currentTimeText.text = "Time: " + FormatTime(playerTimeAlive);

        int remainingLives = Mathf.CeilToInt(currentHealth); // Round up to the nearest integer.
        for (int i = 0; i < lifeImages.Length; i++)
        {
            lifeImages[i].enabled = (i < remainingLives);
        }

        currentGO.text = FormatTime(playerTimeAlive);
        highGO.text = FormatTime(PlayerPrefs.GetFloat("Highscore", 0f));
    }

    #endregion

    #region Coroutines

    private IEnumerator UpdateTimeAlive()
    {
        while (isPlayerAlive)
        {
            // Increase the playerTimeAlive by 1 second
            playerTimeAlive += 1f;

            // Wait for 1 second
            yield return new WaitForSeconds(1f);
        }
    }

    #endregion
}
