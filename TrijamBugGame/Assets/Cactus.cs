using System.Collections;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    private int currentWater = 0;
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

    private SpriteRenderer spriteRenderer;

    Animator anim;

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

        float hideDuration = Time.time - playerHideStartTime;
        if (healthBoosted == false)
        {
            if (hideDuration >= requiredHideTime)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().AddHealth(healthBoostAmount);
                healthBoosted = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Let player Hide
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
        isPlayerHiding = true;
        healthBoosted = false;
        playerHideStartTime = Time.time;
    }

    public void PlayerNotHide()
    {

    }
}
