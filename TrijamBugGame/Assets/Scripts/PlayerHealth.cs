using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3; // Maximum health of the bug
    public int currentHealth; // Current health of the bug

    public float immortalityDuration = 2f; // Duration of temporary immortality after taking damage
    public float flashInterval = 0.2f; // Duration of each flash interval
    private bool isImmortal = false; // Flag to track if the bug is currently immortal

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (!isImmortal)
        {
            currentHealth -= damageAmount;
            if (currentHealth <= 0)
            {
                FindObjectOfType<GameManager>().PlayerDie();
                Die();
            }
            else
            {
                StartCoroutine(TemporaryImmortality());
            }
        }
    }

    private void Die()
    {
        // Implement what should happen when the bug dies (e.g., play death animation, disable controls, etc.)
        // For example, you can call a GameManager method to handle the player's death.
        //GameManager.Instance.OnPlayerDeath();
        Destroy(gameObject);
    }

    private IEnumerator TemporaryImmortality()
    {
        isImmortal = true;

        // Flash the player white during the immortality period
        int numberOfFlashes = Mathf.RoundToInt(immortalityDuration / (2 * flashInterval));
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashInterval);
            spriteRenderer.color = Color.clear;
            yield return new WaitForSeconds(flashInterval);
        }

        spriteRenderer.color = Color.white;
        isImmortal = false;
    }

    public void AddHealth(int amt)
    {
        currentHealth++;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
