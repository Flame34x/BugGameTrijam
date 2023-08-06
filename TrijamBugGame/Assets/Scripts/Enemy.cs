using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    private float currentHealth;
    public float maxHealth;
    private Transform player;

    private void Start()
    {
        currentHealth = maxHealth;
        // Find the player's transform in the scene
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Check if the player is not null (in case the player object is destroyed)
        if (player != null)
        {
            // Calculate the direction from the enemy to the player
            Vector3 directionToPlayer = player.position - transform.position;

            // Normalize the direction vector to get a unit direction
            directionToPlayer.Normalize();

            // Calculate the angle in degrees to face the player
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            // Rotate the enemy to face the player
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

            // Move the enemy towards the player
            transform.position += directionToPlayer * moveSpeed * Time.deltaTime;

            if (transform.position == player.position)
            {
                Die();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cactus"))
        {
            if (player.GetComponent<PlayerMovement>().isHiding)
            {
                Die();
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
                       if (!player.GetComponent<PlayerMovement>().isHiding)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(1);
                Destroy(gameObject);
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cactus"))
        {
            if (player.GetComponent<PlayerMovement>().isHiding)
            {
                Die();
            }
        }
    }


        public void Die()
    {
               // Destroy the enemy
        Destroy(gameObject);
    }
    
}
