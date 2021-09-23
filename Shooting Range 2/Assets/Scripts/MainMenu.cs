using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // When clicking the button, call this function that loads the next scene
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Quits the game through a button on the UI
    public void QuitGame()
    {
        Application.Quit();
    }

}
