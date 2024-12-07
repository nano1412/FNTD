using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;
    public string turretName;
    public int cost;
    public int upgradeCost;
    public int level = 1; // ������������� 1

    [SerializeField] GameObject rangeIndicatorPrefab;
    public GameObject rangeIndicator;
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    public GameObject[] turretPrefabs; // Array �ͧ prefab ����Ѻ���������
    public GameObject bulletPrefab;
    public Transform partToRotate;
    public Transform firePoint;
    public string enemyTag = "Enemy";

    public AudioClip shootSound; // ������§����Ѻ����ԧ
    private AudioSource audioSource; // ��ǤǺ������§
    void Start()
    {
        rangeIndicator = Instantiate(rangeIndicatorPrefab, transform);
        rangeIndicator.SetActive(false);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        audioSource = GetComponent<AudioSource>();
    }

    public int GetCost()
    {
        return cost;
    }

    public void UpgradeTurret()
    {
        // ��Ǩ�ͺ�������Ţͧ�����ѧ���֧�дѺ�٧�ش
        if (level < turretPrefabs.Length)
        {
            int currentPrefabIndex = level - 1;

            // ตรวจสอบว่าเหรียญเพียงพอหรือไม่
            if (CoinSystem.current.SpendCoins(upgradeCost))
            {
                // อัปเกรดเลเวล
                level++;

                // อัปเกรดไปยัง prefab ถัดไป
                UpgradeToNextLevelPrefab(currentPrefabIndex);

                // เพิ่มค่าใช้จ่ายในการอัปเกรด
                upgradeCost += 100;
            }
            else
            {
                // เหรียญไม่พอ ไม่ทำอะไร
                return;
            }
        }
        else
        {
            // เลเวลถึงระดับสูงสุดแล้ว ไม่ทำอะไร
            return;
        }
    }

    private void UpgradeToNextLevelPrefab(int currentPrefabIndex)
    {
        // �纵��˹���С����ع�ͧ�����Ѩ�غѹ
        if (turretPrefabs[currentPrefabIndex + 1] == null)
        {
            // คืนเหรียญให้ผู้เล่นในกรณีที่ prefab ยังไม่ได้ตั้งค่า
            CoinSystem.current.AddCoins(upgradeCost);
            return;
        }

        // เก็บตำแหน่งและการหมุนของป้อมปัจจุบัน
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        // ลบป้อมปัจจุบัน
        Destroy(gameObject);

        // สร้างป้อมใหม่จาก prefab ในเลเวลถัดไป
        GameObject newTurret = Instantiate(turretPrefabs[currentPrefabIndex + 1], currentPosition, currentRotation);

        // กำหนดเลเวลใหม่ให้ป้อมที่อัปเกรดแล้ว
        Turret newTurretScript = newTurret.GetComponent<Turret>();
        if (newTurretScript != null)
        {
            newTurretScript.level = level;
        }
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
        rangeIndicator.transform.localScale = new Vector3(range, rangeIndicator.transform.localScale.y, range);
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
        }

        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
