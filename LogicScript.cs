using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class LogicScript : MonoBehaviour
{
    // Stores the current score of the player, set initially in the Unity Inspector
    public int playerScore;

    // UI elements for displaying the score and game over/win screens
    public Text scoreText;
    public GameObject gameOverScreen;
    public GameObject youWinScreen;

    public Button playAgainButton;  // Reference to the Play Again button

    // Timer text
    public Text timerText;  // Assign in Unity Inspector

    private float elapsedTime;
    private bool isTiming = false;

    private bool hasWon = false;  // Flag to prevent game over after winning

    private bool restartAllowed = false;  // Flag to check if restart is allowed

    private static int rotationState = 180;  // Add rotation state variable that carries over between game restarts

    // Reference to the FaceScript to control the character's appearance
    [SerializeField] 
    private FaceScript faceScript;

    // Variable to store the starting score at the beginning of the game
    private int startingScore;

    void Start()
    {
        // Toggle rotation state
        rotationState = (rotationState == 0) ? 180 : 0;
        faceScript.transform.rotation = Quaternion.Euler(0, 0, rotationState);

        // Store the initial score value at game start to track changes relative to it
        startingScore = playerScore;
        
        // Update the UI to reflect the initial score
        UpdateScoreUI();

        //Timer timing
        elapsedTime = 0f;
        isTiming = false;
    }

    void Update()
    {
        // Start timer when spacebar is pressed
        if (!isTiming && (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            isTiming = true;
        }
        // Timer updating
        if (isTiming)
        {
            elapsedTime += Time.deltaTime;
            timerText.text = FormatTime(elapsedTime);
        }

        // If game over or you win screen is active, restart game on spacebar press
        if (restartAllowed && Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            restartGame();
        }
    }

    // Timer FormatTime details
    string FormatTime(float time)
    {
        int seconds = Mathf.FloorToInt(time);
        int milliseconds = Mathf.FloorToInt((time * 100F) % 100F);
        return string.Format("{0:00}.{1:00}", seconds, milliseconds);
    }

    // Function to decrease the player's score and update UI/character appearance
    [ContextMenu("Increase Score")] // Allows this method to be manually triggered in Unity
    public void addScore(int scoreToAdd)
    {
        // Ensure the score never drops below zero
        playerScore = Mathf.Max(playerScore - scoreToAdd, 0);

        // Update the UI score display
        UpdateScoreUI();

        // Update the character's color based on the new score relative to the starting score
        if (faceScript != null)
        {
            faceScript.UpdateColor(playerScore, startingScore);
        }

        // If the score reaches zero, trigger game over
        if (playerScore <= 0)
        {
            faceScript.faceIsAlive = false;  // Disable character control
        }
    }

    // Function to update the score text in the UI
    private void UpdateScoreUI()
    {
        scoreText.text = playerScore.ToString();
    }

    // Function to restart the game by reloading the current scene
    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Function to handle game over logic
    public void gameOver()
    {
        // If youWin screen is up, don't replace
        if (hasWon) return;

        // Stop timer
        isTiming = false;

        // Display the game over screen UI
        gameOverScreen.SetActive(true);

        restartAllowed = true;  // Allow spacebar to restart the game

        // Disable the character's ability to move by setting faceIsAlive to false
        if (faceScript != null)
        {
            faceScript.faceIsAlive = false;
        }
    }

    // Function to handle win logic
    public void youWin()
    {
        // Check flag so gameOver screen can't replace
        hasWon = true;

        // Stop timer
        isTiming = false;

        // Display the "You Win" screen UI
        youWinScreen.SetActive(true);

        // Replace Game Over screen
        gameOverScreen.SetActive(false);

        // Disable the character's ability to move by setting faceIsAlive to false
        if (faceScript != null)
        {
            faceScript.faceIsAlive = false;
        }

        // Start coroutine for 1sec delay before allowing restart via spacebar
        StartCoroutine(EnableRestartAfterDelay());
    }

        private IEnumerator EnableRestartAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        restartAllowed = true;  // Allow spacebar to restart the game
    }
}
