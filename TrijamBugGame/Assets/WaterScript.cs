using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(1f, 1f, 1f);
    public float scaleTime = 2f;

    private void Start()
    {
        // Start the scaling coroutine on start
        StartCoroutine(ScaleCoroutine());
    }

    private IEnumerator ScaleCoroutine()
    {
        Vector3 initialScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < scaleTime)
        {
            // Calculate the current scale factor based on the elapsed time
            float t = Mathf.Clamp01(elapsedTime / scaleTime);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the object ends with the exact target scale
        transform.localScale = targetScale;
    }
}
