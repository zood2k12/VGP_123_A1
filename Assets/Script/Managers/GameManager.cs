using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    public Action<int> OnLifeValueChanged;

    // Private Lives Variable
    private int _lives = 10;

    // Public variable for getting and setting lives
    public int lives
    {
        get
        {
            return _lives;
        }
        set
        {
            // All lives lost (zero counts as a life due to the check)
            if (value < 0)
            {
                GameOver();
                return;
            }

            // Lost a life
            if (value < _lives)
            {
                Respawn();
            }

            // Cannot roll over max lives
            if (value > maxLives)
            {
                value = maxLives;
            }

            _lives = value;
            OnLifeValueChanged?.Invoke(_lives);

            Debug.Log($"Lives value on {gameObject.name} has changed to {lives}");
        }
    }

    [SerializeField] private int maxLives = 10;
    [SerializeField] private PlayerController playerPrefab;

    [HideInInspector] public PlayerController PlayerInstance => playerInstance;
    private PlayerController playerInstance;
    private Transform currentCheckpoint;
    private MenuController currentMenuController;

    private bool isPaused = false; // Pause state variable

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    void Start()
    {
        // Initialize anything you need on start
    }

    void Update()
    {
        if (!currentMenuController) return;

        // Toggle pause when 'P' is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    // Method to toggle pause and resume
    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // Pauses the game by setting the time scale to 0
            Time.timeScale = 0f;
            currentMenuController.SetActiveState(MenuController.MenuStates.Pause); // Activate pause menu
        }
        else
        {
            // Resumes the game by setting the time scale to 1
            Time.timeScale = 1f;
            currentMenuController.SetActiveState(MenuController.MenuStates.InGame); // Deactivate pause menu
        }

        Debug.Log(isPaused ? "Game Paused" : "Game Resumed");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void GameOver()
    {
        Debug.Log("Game Over, change it to move to a specific level");
        SceneManager.LoadScene("GameOver");
    }

    void Respawn()
    {
        playerInstance.transform.position = currentCheckpoint.position;
    }

    public void SpawnPlayer(Transform spawnLocation)
    {
        playerInstance = Instantiate(playerPrefab, spawnLocation.position, Quaternion.identity);
        currentCheckpoint = spawnLocation;
    }

    public void UpdateCheckpoint(Transform updatedCheckpoint)
    {
        currentCheckpoint = updatedCheckpoint;
    }

    public void SetMenuController(MenuController menuController)
    {
        currentMenuController = menuController;
    }
}
