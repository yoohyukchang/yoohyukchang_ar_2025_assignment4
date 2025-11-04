using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject startMenuCanvas;
    public GameObject inGameCanvas;
    public GameObject youWinCanvas;
    public PlayerController player;
    public CameraController cameraController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // game is paused at the beginning
        Time.timeScale = 0f;

        // Show start menu, hide in-game UI
        startMenuCanvas.SetActive(true);
        inGameCanvas.SetActive(false);
        youWinCanvas.SetActive(false);

        // Disable player movement
        player.enabled = false;
    }

    public void StartGame()
    {
        // Resume gameplay
        Time.timeScale = 1f;

        // Hide start menu, show in-game UI
        startMenuCanvas.SetActive(false);
        inGameCanvas.SetActive(true);

        // Allow player movement
        player.enabled = true;
    }

    public void PlayerReachedGoal()
    {
        youWinCanvas.SetActive(true);
        player.DisablePlayer();
        cameraController.enabled = false;
        player.gameObject.SetActive(false);
    }
}
