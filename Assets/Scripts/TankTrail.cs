using UnityEngine;

public class TankTracks : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    public GameObject trackPrefab;       // Префаб следа из Шага 1
    public Transform leftTrackPoint;     // Точка левой гусеницы
    public Transform rightTrackPoint;    // Точка правой гусеницы

    [Header("Параметры спавна")]
    public float spawnDistance = 0.3f;   // Расстояние движения, через которое оставляется след

    private Vector2 lastSpawnPosition;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastSpawnPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Проверяем, проехал ли танк достаточное расстояние и движется ли он вообще
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

        // Спавним левый и правый след с текущим углом поворота танка
        Instantiate(trackPrefab, leftTrackPoint.position, leftTrackPoint.rotation);
        Instantiate(trackPrefab, rightTrackPoint.position, rightTrackPoint.rotation);
    }
}