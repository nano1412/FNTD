using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;
    public int cost;
    public int upgradeCost;
    public int level = 1; // เริ่มที่เลเวล 1

    public float range = 15f;
    public float fireRate = 1f;
    public int damage = 10;
    private float fireCountdown = 0f;

    public GameObject[] turretPrefabs; // Array ของ prefab สำหรับแต่ละเลเวล
    public GameObject bulletPrefab;
    public Transform partToRotate;
    public Transform firePoint;
    public string enemyTag = "Enemy";

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    public int GetCost()
    {
        return cost;
    }

    public void UpgradeTurret()
    {
        // ตรวจสอบว่าเลเวลของป้อมยังไม่ถึงระดับสูงสุด
        if (level < turretPrefabs.Length) // Use the length of the array to determine max level
        {
            // Store current level for prefab access
            int currentPrefabIndex = level - 1;

            // Upgrade turret level
            level++;
            UpgradeToNextLevelPrefab(currentPrefabIndex); // Pass the current index

            // เพิ่มค่าใช้จ่ายในการอัปเกรด
            upgradeCost += 100; // เพิ่มค่าใช้จ่ายในการอัปเกรด 100 หน่วยทุกครั้ง
        }
        else
        {
            Debug.Log("ป้อมอยู่ในระดับสูงสุดแล้ว!");
        }
    }

    private void UpgradeToNextLevelPrefab(int currentPrefabIndex)
    {
        // เก็บตำแหน่งและการหมุนของป้อมปัจจุบัน
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        // ลบป้อมปัจจุบัน
        Destroy(gameObject);

        // สร้างป้อมใหม่จาก prefab ในเลเวลถัดไป
        // Make sure to use the next level index
        GameObject newTurret = Instantiate(turretPrefabs[currentPrefabIndex + 1], currentPosition, currentRotation);
        newTurret.GetComponent<Turret>().level = level; // กำหนดเลเวลให้ตรงกับป้อมใหม่

        //Debug.Log($"ป้อมอัปเกรดเป็นเลเวล {level}! เปลี่ยนไปใช้ prefab ใหม่");
    }


    private void UpgradeToNextLevelPrefab()
    {
        // เก็บตำแหน่งและการหมุนของป้อมปัจจุบัน
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        // ลบป้อมปัจจุบัน
        Destroy(gameObject);

        // สร้างป้อมใหม่จาก prefab ในเลเวลถัดไป
        GameObject newTurret = Instantiate(turretPrefabs[level - 1], currentPosition, currentRotation);
        newTurret.GetComponent<Turret>().level = level; // กำหนดเลเวลให้ตรงกับป้อมใหม่

        Debug.Log($"ป้อมอัปเกรดเป็นเลเวล {level}! เปลี่ยนไปใช้ prefab ใหม่");
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
            fireCountdown = 1f / fireRate;
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
            bullet.damage = damage;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
