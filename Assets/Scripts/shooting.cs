using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform muzzlePoint;

    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, muzzlePoint.position, muzzlePoint.rotation);
    }
}