using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    // Rigidbody of the player. Don't know why we need that specifically yet.
    private Rigidbody rb;

    private Dictionary<string, int> collected = new Dictionary<string, int>();

    private int count;

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    // Speed at which the player moves.
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();
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
    void SetCountText()
    {
        // Update the count text with the current count.
        countText.text = "Count: " + count.ToString();

        // Check if the count has reached or exceeded the win condition.
        if (count >= 12)
        {
            //Display the win text.
            winTextObject.SetActive(true);

            //Destroy the enemy GameObject
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
    }

    // FixedUpdate is called once per fixed frame-rate frame.
    void FixedUpdate()
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || (collision.gameObject.CompareTag("Lava")))
        {
            // Destroy the current object
            Destroy(gameObject);
            if (!winTextObject.activeSelf)
            {
                winTextObject.gameObject.SetActive(true);
                winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            }
            // Update the winText to display "You Lose!"
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PickUp"))
        {
            //string itemName = other.gameObject.name;

            other.gameObject.SetActive(false);

            count++;

            SetCountText();
        }
    }
}
