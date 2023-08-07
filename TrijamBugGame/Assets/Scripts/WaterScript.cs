using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    public Vector2 targetScale = new Vector2(1f, 1f);
    public float scaleTime = 2f;
    public float attractionForce = 0.5f; // Adjust this value to control the strength of attraction
    public float attractionRange = 0.5f; // Adjust this value to set the range of attraction

    private Transform playerTransform;

    private void Start()
    {
        // Find the player object by tag (you can also assign the player's transform directly if you prefer)
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Start the scaling coroutine on start
        StartCoroutine(ScaleCoroutine());
    }

    private IEnumerator ScaleCoroutine()
    {
        Vector2 initialScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < scaleTime)
        {
            // Calculate the current scale factor based on the elapsed time
            float t = Mathf.Clamp01(elapsedTime / scaleTime);
            transform.localScale = Vector2.Lerp(initialScale, targetScale, t);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // If the player is close enough, apply attraction force
            if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) <= attractionRange)
            {
                Vector2 directionToPlayer = playerTransform.position - transform.position;
                float attractionStrength = Mathf.Lerp(attractionForce, 0f, t); // Gradually reduce the attraction force over time
                GetComponent<Rigidbody2D>().AddForce(directionToPlayer.normalized * attractionStrength, ForceMode2D.Force);
            }

            yield return null;
        }

        // Ensure the object ends with the exact target scale
        transform.localScale = targetScale;
    }
}
