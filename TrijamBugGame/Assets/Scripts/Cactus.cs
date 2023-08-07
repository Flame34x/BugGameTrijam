using System.Collections;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    public int currentWater = 0;
    public int maxWater = 25;

    public Transform hideSpot;

    public int healthBoostAmount = 1;
    public float requiredHideTime = 3f;
    private bool isPlayerHiding = false;
    private float playerHideStartTime = 0;
    private GameObject player;
    private bool healthBoosted = false;

    public Color dryColor = Color.green;
    public Color rottenColor;

    public float waterLossInterval = 1f;
    public int waterLossAmount = 1;
    private float hideDuration;

    private SpriteRenderer spriteRenderer;

    Animator anim;

    public bool isAlive = true;
    private bool hasDied = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentWater = maxWater;
        UpdateCactusColor();

        // Start the water loss coroutine
        StartCoroutine(WaterLossCoroutine());
    }

    private void Update()
    {
        UpdateAnimation();
        if (isAlive)
        {

            if (isPlayerHiding) // Only apply health boost if the player is hiding in the cactus
            {
                hideDuration = Time.time - playerHideStartTime;
                if (!healthBoosted)
                {
                    if (hideDuration >= requiredHideTime)
                    {
                        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().isHiding)
                        {
                            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().AddHealth(healthBoostAmount);
                            healthBoosted = true;
                        }
                    }
                }
            }
        }

        if (!isAlive)
        {
            if (!hasDied)
            {
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CactusDie();
                hasDied = true;
            }
        }

        if (currentWater == 0)
        {
            isAlive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Let player Hide
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerNotHide();
        }
    }

    public void AddWater(int amt)
    {
        
        currentWater += amt;
        currentWater = Mathf.Clamp(currentWater, 0, maxWater);

        UpdateCactusColor();
    }

    private void UpdateCactusColor()
    {
        // Calculate the percentage of current water relative to the max water
        float waterPercentage = (float)currentWater / maxWater;

        // Interpolate between rottenColor and dryColor based on the water percentage
        spriteRenderer.color = Color.Lerp(rottenColor, dryColor, waterPercentage);
    }

    private IEnumerator WaterLossCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(waterLossInterval);

            // Reduce the current water amount
            currentWater -= waterLossAmount;
            currentWater = Mathf.Clamp(currentWater, 0, maxWater);

            UpdateCactusColor();
        }
    }

    private void UpdateAnimation()
    {
        anim.SetInteger("Water", currentWater);
    }

    public void PlayerHide(GameObject player)
    {
        hideDuration = 0;
        isPlayerHiding = true;
        healthBoosted = false;
        playerHideStartTime = Time.time;
    }

    public void PlayerNotHide()
    {
        // Reset health boost-related variables
        isPlayerHiding = false;
        healthBoosted = false;
    }
}
