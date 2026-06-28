using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private float checkTimer = 0.5f;
    private float timer = 0f;

    // Задержка в 1 секунду перед началом проверок врагов
    private float startDelay = 1.0f;
    private float startTimer = 0f;

    void Update()
    {
        // Ждем, пока пройдет секунда после старта сцены
        if (startTimer < startDelay)
        {
            startTimer += Time.deltaTime;
            return; // Выходим из Update, ничего не проверяя
        }

        // Проверка ботов по таймеру
        timer += Time.deltaTime;
        if (timer >= checkTimer)
        {
            timer = 0f;
            CheckEnemies();
        }
    }

    void CheckEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            if (currentSceneName == "Level1")
            {
                SceneManager.LoadScene("WinScene");
            }
            else if (currentSceneName == "Level2")
            {
                SceneManager.LoadScene("FinalScene");
            }
        }
    }
}