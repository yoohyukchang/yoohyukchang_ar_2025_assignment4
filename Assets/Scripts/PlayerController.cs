using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public int total = 0;
    public float speed = 0f;
    public float jumpForce = 0f;

    public TextMeshProUGUI counterText;
    public GameObject powerUpCapsule;
    public GameObject poweredUpTextGameObject;
    public GameObject finishGoal;

    private bool isGrounded = true;
    private int numCollected = 0;
    private bool isPoweredUp = false;

    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody rb;
    private Renderer playerRenderer;
    private GameManager gameManager;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        rb = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();

        gameManager = FindFirstObjectByType<GameManager>();
        

        powerUpCapsule.SetActive(false);
        poweredUpTextGameObject.SetActive(false);

        counterText.text = numCollected + " / " + total;
    }

    private void Update()
    {
        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);

        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player has landed on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // handle breaking the wall
        if (collision.gameObject.CompareTag("BreakableWall") && isPoweredUp)
        {
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            other.gameObject.SetActive(false);

            numCollected++;
            counterText.text = numCollected + " / " + total;

            if (numCollected >= total)
            {
                powerUpCapsule.SetActive(true);
            }
        }
        else if (other.CompareTag("PowerUpCapsule"))
        {   
            // display "Powered Up!" text
            poweredUpTextGameObject.SetActive(true);

            // get the capsule's color
            Renderer capsuleRenderer = other.GetComponent<Renderer>();
            Color capsuleColor = capsuleRenderer.material.color;

            // change the player's color to the capsule's color
            playerRenderer.material.color = capsuleColor;

            // hide the capsule and make the speed 6 times faster
            other.gameObject.SetActive(false);
            speed *= 6;

            isPoweredUp = true;
        }
        else if (other.CompareTag("FinishGoal"))
        {
            // notify the game manager to handle end-of-game logic
            gameManager.PlayerReachedGoal();
        }
    }

    // public method to let the game manager to disable the player
    public void DisablePlayer()
    {
        this.enabled = false;
    }

}
