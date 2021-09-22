using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public bool pauseMenuActive = false;
    public GameObject options;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused && pauseMenuActive)
            {
                Resume();
                
            }
                
            else if (!GameIsPaused)
            {
                Pause();
                
            }

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

    void DisableOptions()
    {
        options.SetActive(false);
        pauseMenuUI.SetActive(true);
        pauseMenuActive = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuActive = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenuActive = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
