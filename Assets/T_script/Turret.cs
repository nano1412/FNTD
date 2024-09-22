using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;

    public float range = 15f;
    public float fireRate = 1f;
    public int damage = 10;
    private float fireCountdown = 0f;

    public GameObject bulletPrefab;
    public Transform partToRotate;
    public Transform firePoint;
    public string enemyTag = "Enemy";

    // ระดับของป้อม
    public int level = 1;

    // คุณสมบัติของป้อมที่แต่ละระดับ
    private int[] levelDamage = { 10, 20, 30 };  // ดาเมจของแต่ละระดับ
    private float[] levelFireRate = { 1f, 1.5f, 2f };  // อัตราการยิงของแต่ละระดับ

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (target == null)
            return;

        LockOnTarget();

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / levelFireRate[level - 1];  // ใช้ fireRate ตามระดับ
        }

        fireCountdown -= Time.deltaTime;
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
            bullet.damage = levelDamage[level - 1];  // กำหนดดาเมจตามระดับ
        }
    }

    // ฟังก์ชันสำหรับอัปเกรดป้อม
    public void UpgradeTurret()
    {
        if (level < 3)  // มีทั้งหมด 3 ระดับ
        {
            level++;
            Debug.Log("Turret upgraded to level " + level);
        }
        else
        {
            Debug.Log("Turret is already at max level.");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}