using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string winSceneName = "WinScene";

    private bool isLevelFinished = false;

    void Update()
    {
        if (isLevelFinished) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        isLevelFinished = true;

        if (Application.CanStreamedLevelBeLoaded(winSceneName))
        {
            SceneManager.LoadScene(winSceneName);
        }
    }
}