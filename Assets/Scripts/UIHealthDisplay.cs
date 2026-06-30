using UnityEngine;
using TMPro;

public class UIHealthDisplay : MonoBehaviour
{
    public Health playerHealth;

    private TextMeshProUGUI healthText;

    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();

        if (playerHealth == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
            }
        }
    }

    void Update()
    {
        if (playerHealth != null && healthText != null)
        {
            healthText.text = $"Здоровье игрока: {(int)playerHealth.currentHealth}/{(int)playerHealth.maxHealth}";
        }
        else if (playerHealth == null && healthText != null)
        {
            healthText.text = "Здоровье игрока: 0";
        }
    }
}