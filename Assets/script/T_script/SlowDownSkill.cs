using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlowDownSkill : MonoBehaviour
{
    public Button slowDownButton; // ปุ่ม UI สำหรับใช้สกิล Slow Down
    public int skillCost = 150; // ราคาของสกิล
    public float slowDownDuration = 5f; // ระยะเวลาที่ศัตรูจะถูกลดความเร็ว
    public float slowFactor = 0.5f; // อัตราการลดความเร็ว (50% ของความเร็วเดิม)
    public float cooldownTime = 20f; // เวลา Cooldown 20 วินาที

    private bool isCooldown = false; // ตรวจสอบว่ากำลังอยู่ในช่วง Cooldown หรือไม่
    private float cooldownTimer; // ตัวจับเวลาสำหรับ Cooldown

    void Start()
    {
        if (slowDownButton != null)
        {
            slowDownButton.onClick.AddListener(ActivateSkill);
        }
    }

    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            slowDownButton.interactable = false; // ปิดการใช้งานปุ่มในช่วง Cooldown

            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
                slowDownButton.interactable = true; // เปิดการใช้งานปุ่มเมื่อ Cooldown สิ้นสุด
            }
        }
    }

    void ActivateSkill()
    {
        if (isCooldown)
        {
            Debug.Log("Skill is on cooldown.");
            return;
        }

        if (CoinSystem.current.SpendCoins(skillCost))
        {
            Debug.Log("Skill Slow Down activated! Slowing all enemies.");
            ApplySlowDown(); // ลดความเร็วศัตรูทุกตัว
            StartCooldown(); // เริ่ม Cooldown
        }
        else
        {
            Debug.Log("Not enough coins to use Slow Down.");
        }
    }

    private void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = cooldownTime;
        Debug.Log($"Slow Down skill on cooldown for {cooldownTime} seconds.");
    }

    void ApplySlowDown()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>(); // หา Enemy ทุกตัวในฉาก

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.moveSpeed *= slowFactor; // ลดความเร็วลงตาม slowFactor
                Debug.Log($"{enemy.name} speed reduced.");
            }
        }

        StartCoroutine(ResetEnemySpeedAfterDuration());
    }

    private IEnumerator ResetEnemySpeedAfterDuration()
    {
        yield return new WaitForSeconds(slowDownDuration);

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.moveSpeed /= slowFactor; // คืนค่าความเร็วปัจจุบัน
                Debug.Log($"{enemy.name} speed restored.");
            }
        }
    }
}
