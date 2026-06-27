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
            rb.linearVelocity = moveDirection * moveSpeed;

            if (moveDirection != Vector2.zero)
            {
                Vector2 lookTarget = (Vector2)transform.position + moveDirection;
                RotateWithPhysics(lookTarget);
            }

            positionCheckTimer += Time.fixedDeltaTime;

            if (positionCheckTimer >= 0.15f)
            {
                float distanceMoved = Vector2.Distance(transform.position, lastPosition);

                if (distanceMoved < 0.05f)
                {
                    ChooseRandomDirection(true);
                }

                lastPosition = transform.position;
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
        if (!canSeePlayer)
        {
            ChooseRandomDirection(true);
        }
    }
}