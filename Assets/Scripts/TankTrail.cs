using UnityEngine;

public class TankTracks : MonoBehaviour
{
    [Header("—сылки на объекты")]
    public GameObject trackPrefab;
    public Transform leftTrackPoint; 
    public Transform rightTrackPoint; 

    [Header("ѕараметры спавна")]
    public float spawnDistance = 0.3f; 

    private Vector2 lastSpawnPosition;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastSpawnPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            float distanceMoved = Vector2.Distance(transform.position, lastSpawnPosition);

            if (distanceMoved >= spawnDistance)
            {
                SpawnTracks();
                lastSpawnPosition = transform.position;
            }
        }
    }

    void SpawnTracks()
    {
        if (trackPrefab == null || leftTrackPoint == null || rightTrackPoint == null) return;

        Instantiate(trackPrefab, leftTrackPoint.position, leftTrackPoint.rotation);
        Instantiate(trackPrefab, rightTrackPoint.position, rightTrackPoint.rotation);
    }
}   