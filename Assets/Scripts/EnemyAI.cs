using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float directionCooldown = 0.5f;
    private float lastDirectionChangeTime = 0f;

    public GameObject projectilePrefab;
    public Transform muzzlePoint;
    public float fireRate = 1.5f;
    private float nextFireTime = 0f;

    public float visionRange = 10f;
    private Transform playerTransform;
    private bool canSeePlayer = false;

    private float stuckTimer = 0f;

    private Vector2 lastPosition;
    private float positionCheckTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        ChooseRandomDirection();
    }

    void Update()
    {
        if (playerTransform == null) return;

        CheckVisibility();

        if (canSeePlayer)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void FixedUpdate()
    {
        if (canSeePlayer)
        {
            RotateWithPhysics(playerTransform.position);
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            rb.linearVelocity = directionToPlayer * (moveSpeed * 0.6f);
            stuckTimer = 0f;
        }
        else
        {
            // Едем строго по направлению
            rb.linearVelocity = moveDirection * moveSpeed;

            if (moveDirection != Vector2.zero)
            {
                Vector2 lookTarget = (Vector2)transform.position + moveDirection;
                RotateWithPhysics(lookTarget);
            }

            // --- НАДЁЖНАЯ ПРОВЕРКА НА ЗАСТРЕВАНИЕ ---
            positionCheckTimer += Time.fixedDeltaTime;

            // Каждые 0.15 секунд проверяем, сдвинулся ли танк с места
            if (positionCheckTimer >= 0.15f)
            {
                float distanceMoved = Vector2.Distance(transform.position, lastPosition);

                // Если за 0.15 сек танк проехал меньше чем 0.05 юнита (практически стоит на месте)
                if (distanceMoved < 0.05f)
                {
                    ChooseRandomDirection(true); // Принудительно меняем направление
                }

                lastPosition = transform.position; // Запоминаем текущую позицию
                positionCheckTimer = 0f;
            }
        }
    }



    void CheckVisibility()
    {
        Vector2 directionToPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= visionRange)
        {
            int layerMask = LayerMask.GetMask("Default", "Player");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, visionRange, layerMask);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player") || hit.collider.transform.root.CompareTag("Player"))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green);
                    canSeePlayer = true;
                    return;
                }
            }
        }

        if (canSeePlayer)
        {
            canSeePlayer = false;
            ChooseRandomDirection();
        }
    }

    void RotateWithPhysics(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        if (direction.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.MoveRotation(targetAngle);
        }
    }

    void ChooseRandomDirection(bool force = false)
    {
        if (!force)
        {
            if (Time.time - lastDirectionChangeTime < directionCooldown) return;
        }

        lastDirectionChangeTime = Time.time;

        Vector2 oldDirection = moveDirection;

        int attempts = 0;
        while (moveDirection == oldDirection && attempts < 10)
        {
            attempts++;
            int randomIndex = Random.Range(0, 4);
            switch (randomIndex)
            {
                case 0: moveDirection = Vector2.up; break;
                case 1: moveDirection = Vector2.down; break;
                case 2: moveDirection = Vector2.left; break;
                case 3: moveDirection = Vector2.right; break;
            }
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || muzzlePoint == null) return;

        GameObject spawnedProjectile = Instantiate(projectilePrefab, muzzlePoint.position, muzzlePoint.rotation);
        Rigidbody2D projectileRb = spawnedProjectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            projectileRb.linearVelocity = muzzlePoint.up * 10f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu")
        {
            // Если врезались в стену или препятствие
            if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.name.Contains("Wall"))
            {
                // Вызываем ваш метод смены направления/разворота бота
                ChooseRandomDirection();
            }
        }
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.name.Contains("Wall"))
        {
            // Получаем точку контакта со стеной
            ContactPoint2D contact = collision.contacts[0];

            // Вычисляем нормаль (куда «смотрит» стена в точке удара)
            Vector2 wallNormal = contact.normal;

            // Вычисляем вектор отражения (как мячик отскакивает от стены)
            Vector2 reflectDirection = Vector2.Reflect(moveDirection, wallNormal);

            // Добавляем небольшую случайность, чтобы бот не циклился между двумя стенами
            reflectDirection += new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));

            // Задаем новое безопасное направление, направленное ОТ угла
            moveDirection = reflectDirection.normalized;
        }
    }
}