using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float lifetime = 2f;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    }