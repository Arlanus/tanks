using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float lifetime = 2f; // Время жизни следа в секундах
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    }