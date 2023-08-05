using System.Collections;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    private int currentWater = 0;
    public int maxWater = 25;

    public Transform hideSpot;

    public Color dryColor = Color.green;
    public Color rottenColor;

    public float waterLossInterval = 1f;
    public int waterLossAmount = 1;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentWater = maxWater;
        UpdateCactusColor();

        // Start the water loss coroutine
        StartCoroutine(WaterLossCoroutine());
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
}
