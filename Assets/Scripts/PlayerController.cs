using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 5;
    public HealthScript healthScript;
    public EmptyHoneycombScript emptyHoneycombScript;

    public int emptyHoneycombPieceCount = 0;    // emptyHoneycombPieceCount is for the healthbar in the story mode.

    private bool isInvulnerable = false;
    private float invulnerabilityDuration = 1.0f; // seconds of i-frame time
    private bool touchingDamageSource = false;

    private Rigidbody rb;                       // Rigidbody of the player. Don't know why we need that specifically yet.

    private int count;                          // count is for the objects in the score mode

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    // Speed at which the player moves.
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject WinLoseRetryButton;
    public GameObject WinLoseQuitButton;
    public GameObject winTextObject;
    public GameObject RetryButton;
    public GameObject QuitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();

        currentHealth = maxHealth;
        healthScript.UpdateHealthBar(currentHealth, maxHealth);


        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
    }

    // This function is called when a move input is detected.
    void OnMove (InputValue movementValue)
    {
        // Connect the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>(); // use <> when you are getting a type. Vector2 is a type of ... component tecnically class

        // Store the X and Y components of the movement.
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Function to update the displayed count of "PickUp" objects collected.
    void SetCountText() //handles count, winning scoremode, win text, and deleting enemies on win
    {
        countText.text = "Count: " + count.ToString();          // Update the count text with the current count.

        if (count >= 12)
        {
            winTextObject.SetActive(true);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
            WinLoseRetryButton.SetActive(true);
            WinLoseQuitButton.SetActive(true);
        }
    }

    // FixedUpdate is called once per fixed frame-rate frame.
    void FixedUpdate()  //Camera, Player movenment
    {
        // Get camera forward and right directions and flatten them on the XZ plane
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;          // remove vertical component
        camForward.Normalize();     // make it unit length

        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        // Combine input with camera directions
        Vector3 movement = camForward * movementY + camRight * movementX;

        // Apply force to move the player
        rb.AddForce(movement * speed);

        // Optional: rotate the player to face movement direction
        if (movement.magnitude > 0.1f)
        {
            rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(movement), 0.2f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            touchingDamageSource = false;
    }

    private void Update()
    {
        if (touchingDamageSource)
            TryTakeDamage();
    }

    private void OnCollisionEnter(Collision collision) //Check for damage, display you lose
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            currentHealth = Mathf.Clamp(currentHealth - 16, 0, maxHealth);
            healthScript.UpdateHealthBar(currentHealth, maxHealth);
            ShowLoseUI();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            touchingDamageSource = true;
            TryTakeDamage();
        }
    }

    private void TryTakeDamage()
    {
        if (isInvulnerable) return;

        currentHealth = Mathf.Clamp(currentHealth - 1, 0, maxHealth);
        healthScript.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            ShowLoseUI();
        }
        else
        {
            StartCoroutine(InvulnerabilityFrames());
        }
    }

    private IEnumerator InvulnerabilityFrames()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    private void OnTriggerEnter(Collider other) // check if you touched a pickup, update count
    {
        if(other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);

            count++;

            SetCountText();
        }

        if(other.gameObject.CompareTag("Extra Honeycomb Piece"))
        {
            other.gameObject.SetActive(false);                      // disable the object
            emptyHoneycombPieceCount++;                             // increase the count
            emptyHoneycombScript.UpdateEmptyHoneycombUI(emptyHoneycombPieceCount);
        }
    }
    private void ShowLoseUI()
    {
        if (!winTextObject.activeSelf)
        {
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            WinLoseRetryButton.SetActive(true);
            WinLoseQuitButton.SetActive(true);
        }

        Destroy(gameObject); // Only destroy player once
    }
}
