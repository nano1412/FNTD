using System.Collections.Generic;
using UnityEngine;

public class Sanctuary : MonoBehaviour
{
    public float range = 10f;           // ระยะทำดาเมจรอบตัว
    public int damage = 20;             // จำนวนดาเมจที่ทำ
    public float fireRate = 1f;         // อัตราการยิง (จำนวนครั้งต่อวินาที)
    public string enemyTag = "Enemy";   // แท็กศัตรู

    public int cost;                    // ค่าใช้จ่ายในการวาง Sanctuary
    public int upgradeCost;             // ค่าใช้จ่ายในการอัปเกรด Sanctuary
    public int level = 1;               // เริ่มที่เลเวล 1
    public GameObject[] sanctuaryPrefabs; // Array ของ prefab สำหรับแต่ละเลเวล

    private Transform target;           // เป้าหมายศัตรู
    private float fireCountdown = 0f;   // ตัวนับถอยหลังสำหรับอัตราการยิง

    void Start()
    {
        // เริ่มต้นการหาศัตรูทุก 0.5 วินาที
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void Update()
    {
        // ถ้าไม่มีเป้าหมาย ไม่ต้องทำงาน
        if (target == null)
            return;

        // เมื่อถึงเวลายิง
        if (fireCountdown <= 0f)
        {
            ExplodeAOE(); // โจมตีในบริเวณรอบตัว
            fireCountdown = 1f / fireRate; // ตั้งเวลาการยิงครั้งถัดไป
        }

        fireCountdown -= Time.deltaTime;
    }

    void UpdateTarget()
    {
        // หาเป้าหมายศัตรูที่อยู่ในระยะ
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // ถ้าศัตรูอยู่ในระยะ จะตั้งค่าเป็นเป้าหมาย
        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void ExplodeAOE()
    {
        // หาเป้าหมายศัตรูในระยะ
        if (target == null) return;

        // ระเบิดที่ตำแหน่งของศัตรูเป้าหมาย
        Collider[] hitEnemies = Physics.OverlapSphere(target.position, range);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag(enemyTag))
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.TakeDamage(damage); // ทำดาเมจให้ศัตรู
                }
            }
        }
    }

    // ฟังก์ชันสำหรับการอัปเกรด Sanctuary (เหมือนกับที่ใช้ใน Turret)
    public void UpgradeSanctuary()
    {
        if (level < sanctuaryPrefabs.Length)
        {
            int currentPrefabIndex = level - 1; // เก็บอินเด็กซ์ปัจจุบันสำหรับ prefab
            level++;
            UpgradeToNextLevelPrefab(currentPrefabIndex); // อัปเกรดไปยัง prefab ถัดไป
            upgradeCost += 100; // เพิ่มค่าใช้จ่ายในการอัปเกรด
        }
        else
        {
            Debug.Log("Sanctuary อยู่ในระดับสูงสุดแล้ว!");
        }
    }

    private void UpgradeToNextLevelPrefab(int currentPrefabIndex)
    {
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;
        Destroy(gameObject);

        GameObject newSanctuary = Instantiate(sanctuaryPrefabs[currentPrefabIndex + 1], currentPosition, currentRotation);
        newSanctuary.GetComponent<Sanctuary>().level = level;
        Debug.Log($"Sanctuary อัปเกรดเป็นเลเวล {level}! เปลี่ยนไปใช้ prefab ใหม่");
    }

    // แสดงระยะการทำดาเมจ AOE บน Scene View
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
