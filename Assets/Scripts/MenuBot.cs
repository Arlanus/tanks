using UnityEngine;

public class MenuBot : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    private float stuckTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        rb.linearDamping = 0f;
        rb.angularDamping = 0f;

        ChooseRandomDirection();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;

        if (moveDirection != Vector2.zero)
        {
            Vector2 lookTarget = (Vector2)transform.position + moveDirection;
            RotateWithPhysics(lookTarget);
        }

        if (rb.linearVelocity.magnitude < 0.5f)
        {
            stuckTimer += Time.fixedDeltaTime;
            if (stuckTimer >= 0.15f)
            {
                ChooseRandomDirection();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
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

    void ChooseRandomDirection()
    {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ChooseRandomDirection();
    }
}