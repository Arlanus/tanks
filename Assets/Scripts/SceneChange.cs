using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Level1");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void RestartGame2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void NextLevel()
    {
        SceneManager.LoadScene("Level2");
    }
    public void Quit()
    {
        Application.Quit();
    }
}