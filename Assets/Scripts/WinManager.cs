using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    private float checkTimer = 0.5f; // Интервал проверки, чтобы не перегружать Update
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkTimer)
        {
            timer = 0f;
            CheckEnemies();
        }
    }

    void CheckEnemies()
    {
        // Ищем всех живых ботов с тегом Enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Если ботов на уровне больше нет
        if (enemies.Length == 0)
        {
            // Получаем имя текущей сцены
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