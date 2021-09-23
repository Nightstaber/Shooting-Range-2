using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPauseMenu : MonoBehaviour
{
    // Bool for game is paused
    public static bool GameIsPaused = false;

    // GameObjects to enable / disable
    [SerializeField]
    GameObject pauseMenuUI;
    bool pauseMenuActive = false;
    [SerializeField]
    GameObject options;


    // Update is called once per frame
    void Update()
    {

        // Look for Escape button input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If the game is pause and the pausemenu is active, resume the game.
            if (GameIsPaused && pauseMenuActive)
            {
                Resume();
                
            }
                
            // If the game is not paused (running), pause the game
            else if (!GameIsPaused)
            {
                Pause();
                
            }

            // If the game is paused, but not in the pausemenu, it should disable the options screen to get back to pausemenu
            else
            {
                DisableOptions();
            }
            
        }
    }

    public void OptionsBool(bool active)
    {
        pauseMenuActive = active;
    }

    // Leave options screen, go back to pause menu.
    void DisableOptions()
    {
        options.SetActive(false);
        pauseMenuUI.SetActive(true);
        pauseMenuActive = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Resumes the game, deactivating pausemenu, setting timescale to 1 and locking the cursor
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuActive = false;
    }

    // Pause the game, enable the pausemenu, unlock mouse cursor
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenuActive = true;
    }
    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
