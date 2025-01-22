using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FaceScript : MonoBehaviour
{
    // Reference to the Rigidbody2D component controlling movement
    public Rigidbody2D myRigidbody;
    
    // Strength of the flap movement when pressing space
    public float flapStrength;

    // Flag to control whether the player can still move
    public bool faceIsAlive = true;

    // Reference to the SpriteRenderer component for modifying color
    private SpriteRenderer spriteRenderer;

    public LogicScript logic;

    // Reference to the instruction text
    public Text instructionText;

    // Prevents game from starting until input is received
    private bool gameStarted = false;

    void Start()
    {
        // Get and store the SpriteRenderer component for later color changes
        spriteRenderer = GetComponent<SpriteRenderer>();

        logic = FindAnyObjectByType<LogicScript>();
        
        if (logic == null)
        {
            Debug.LogError("LogicScript not found in the scene! Make sure it's added to a GameObject.");
        }

        // Disable movement until input is detected
        myRigidbody.linearVelocity = Vector2.zero;
        myRigidbody.simulated = false;  // Prevent physics calculations until the game starts
    }

    void Update()
    {
        // Start the game only when the player presses Space or clicks
        if (!gameStarted && (Input.GetKeyDown(KeyCode.Space)) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            gameStarted = true;  // Allow the game to start

            // Hide the instruction text
            if (instructionText != null)
            {
                instructionText.gameObject.SetActive(false);
            }

            // Enable physics and apply initial jump
            myRigidbody.simulated = true;
            myRigidbody.linearVelocity = Vector2.up * flapStrength;
        }

        // Allow player movement only after the game has started
        if (gameStarted && faceIsAlive && Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            myRigidbody.linearVelocity = Vector2.up * flapStrength;
        }

    }

        // Game Over and You Win screens
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            logic.gameOver();
            faceIsAlive = false;
        }

        if (collision.gameObject.layer == 7)
        {
            logic.youWin();
            faceIsAlive = false;
        }
    }

    // Function to update the color of the sprite based on score changes
    public void UpdateColor(int currentScore, int startingScore)
    {
        // Calculate score ratio between current score and starting score (0 to 1)
        float scoreRatio = (float)currentScore / startingScore;

        // Define the color range: from dark (black) at zero score to bright (white) at full score
        Color baseColor = Color.white;  // Original color when score is full
        Color darkerColor = Color.black;  // Darkest color when score is zero

        // Interpolate the color between dark and bright based on score ratio
        spriteRenderer.color = Color.Lerp(darkerColor, baseColor, scoreRatio);
    }
}
