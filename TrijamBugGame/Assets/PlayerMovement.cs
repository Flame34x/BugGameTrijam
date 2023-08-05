using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    #region Movement Variables

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    #endregion

    #region Water Collection Variables

    [Header("Water Collection Settings")]
    public int maxWaterToHold = 5;
    public int amtOfWater { get; protected set; }


    #endregion

    #region Hiding and Interaction Variables

    [Header("Hiding and Interaction Settings")]
    public bool isHiding = false;
    private bool isNearCactus = false;
    private GameObject cactusNearby;

    #endregion

    private SpriteRenderer sr;
    public static PlayerMovement Instance;


    #region Unity Callbacks

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Instance = this;
    }

    private void Update()
    {
        HandleMovement();

        if (isNearCactus)
        {
            HandleCactusInteractions();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            HandleWaterPickup(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Cactus"))
        {
            HandleCactusEnter(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cactus"))
        {
            HandleCactusExit();
        }
    }

    #endregion

    #region Movement Methods

    private void HandleMovement()
    {
        if (!isHiding)
        {
            // Input values for horizontal and vertical axes
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calculate the movement vector
            Vector2 movement = new Vector2(horizontalInput, verticalInput);

            // Set the velocity of the Rigidbody to move the player
            rb.velocity = movement * moveSpeed;

            // If you want to limit the player to move at the same speed in all directions,
            // you can normalize the movement vector and then multiply it by the moveSpeed:
            // rb.velocity = movement.normalized * moveSpeed;
        }
        if (isHiding)
        {
            rb.velocity = new Vector2(0,0);
        }
    }

    #endregion

    #region Cactus Interaction Methods

    private void HandleCactusInteractions()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Hide();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DepositWater(cactusNearby.GetComponent<Cactus>());
        }
    }

    private void Hide()
    {
        if (cactusNearby != null)
        {
            if (!isHiding)
            {
                isHiding = true;
                transform.position = cactusNearby.GetComponent<Cactus>().hideSpot.position;
                sr.enabled = false;
            }
             else
            {
                
                isHiding = false;
                sr.enabled = true;
            }
        
        }
    }

    private void DepositWater(Cactus cactusToFeed)
    {
        cactusToFeed.AddWater(amtOfWater);
        amtOfWater = 0;
    }

    #endregion

    #region Trigger Events Methods

    private void HandleWaterPickup(GameObject water)
    {
        if (amtOfWater < maxWaterToHold)
        {
            amtOfWater++;
            Destroy(water);
        }
    }

    private void HandleCactusEnter(GameObject cactus)
    {
        isNearCactus = true;
        cactusNearby = cactus;
    }

    private void HandleCactusExit()
    {
        isNearCactus = false;
        cactusNearby = null;
    }

    #endregion
}
