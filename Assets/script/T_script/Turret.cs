using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;
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
            int currentPrefabIndex = level - 1; // ���Թ�硫�Ѩ�غѹ����Ѻ prefab

            // ��Ǩ�ͺ�������­��§���������
            if (CoinSystem.current.SpendCoins(upgradeCost))
            {
                // �ѻ�ô�����
                level++;
                UpgradeToNextLevelPrefab(currentPrefabIndex); // �ѻ�ô��ѧ prefab �Ѵ�

                // ������������㹡���ѻ�ô
                upgradeCost += 100; // ���� 100 ˹��·ء���駷���ѻ�ô

                Debug.Log($"�����ѻ�ô������� {level} ����! ������­ {upgradeCost} ˹���");
            }
            else
            {
                Debug.Log("����­��������Ѻ����ѻ�ô");
            }
        }
        else
        {
            Debug.Log("����������дѺ�٧�ش����!");
        }
    }

    private void UpgradeToNextLevelPrefab(int currentPrefabIndex)
    {
        // �纵��˹���С����ع�ͧ�����Ѩ�غѹ
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        // ź�����Ѩ�غѹ
        Destroy(gameObject);

        // ���ҧ��������ҡ prefab �����ŶѴ�
        GameObject newTurret = Instantiate(turretPrefabs[currentPrefabIndex + 1], currentPosition, currentRotation);
        newTurret.GetComponent<Turret>().level = level; // ��˹���������ç�Ѻ��������

        Debug.Log($"�����ѻ�ô������� {level}! ����¹��� prefab ����");
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
