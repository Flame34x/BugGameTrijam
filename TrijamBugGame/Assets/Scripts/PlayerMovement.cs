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
    private Vector2 lastMovementDirection = Vector2.zero;


    #endregion

    #region Water Collection Variables

    [Header("Water Collection Settings")]
    public int amtOfWater;


    #endregion

    public GameObject tooltip;

    #region Hiding and Interaction Variables

    [Header("Hiding and Interaction Settings")]
    public bool isHiding = false;
    private bool isNearCactus = false;
    private GameObject cactusNearby;

    #endregion

    private SpriteRenderer sr;
    public static PlayerMovement Instance;
    private Animator anim;


    #region Unity Callbacks

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Instance = this;
    }

    private void Update()
    {
        HandleMovement();
        HandleAnimation();

        if (isNearCactus)
        {
            HandleCactusInteractions();
            ShowCactusTooltip(true);
        }
        else
        {
            ShowCactusTooltip(false);
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
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;

            if (movement.magnitude > 0.01f)
            {
                lastMovementDirection = movement;
            }

            rb.velocity = movement * moveSpeed;

            // Update sprite rotation
            if (movement.magnitude > 0.01f)
            {
                float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void HandleAnimation()
    {
        if (rb.velocity.x != 0 && rb.velocity.y != 0)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
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
                cactusNearby.GetComponent<Cactus>().PlayerHide(gameObject);
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

        amtOfWater++;
        Destroy(water);

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

    private void ShowCactusTooltip(bool show)
    {
        tooltip.SetActive(show);

        if (show)
        {
            // Calculate the offset to make the tooltip hover above the player
            float yOffset = 1.5f; // Adjust this value to set the desired height above the player

            // Calculate the position of the tooltip relative to the player position
            Vector3 tooltipPosition = transform.position + new Vector3(0f, yOffset, 0f);

            // Set the tooltip position
            tooltip.transform.position = tooltipPosition;
        }
    }
}
