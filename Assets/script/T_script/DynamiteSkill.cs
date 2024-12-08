using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DynamiteSkill : MonoBehaviour
{
    public GameObject effect;
    public DamageType damageType;
    public int skillCost = 100; // ค่าความต้องการเหรียญของสกิล
    public float skillDamage = 50; // ความเสียหายที่ทำได้
    public float cooldownTime = 30f; // เวลา Cooldown 30 วินาที
    public float explosionRadius = 5f; // รัศมีของการระเบิด AOE

    private bool isCooldown = false; // ตรวจสอบว่าสกิลกำลังอยู่ในช่วง Cooldown หรือไม่
    [SerializeField] private float cooldownTimer; // ตัวจับเวลา Cooldown

    public Button skillButton; // ปุ่ม UI ของสกิล
    private Color originalColor; // สีเดิมของปุ่ม
    private Color cooldownColor = Color.red; // สีที่ใช้แสดงช่วง Cooldown

    void Start()
    {
        // เก็บสีเดิมของปุ่ม
        originalColor = skillButton.image.color;
    }

    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                EndCooldown();
            }
        }
    }

    public void ActivateSkill()
    {
        if (isCooldown)
        {
            return;
        }

        if (CoinSystem.current.SpendCoins(skillCost))
        {
            StartCoroutine(SelectLocationAndDamage());
            StartCooldown();
        }
    }

    private void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = cooldownTime;

        // เปลี่ยนสีปุ่มและปิดการใช้งาน
        skillButton.image.color = cooldownColor;
        skillButton.interactable = false;
    }

    private void EndCooldown()
    {
        isCooldown = false;

        // คืนสีเดิมของปุ่มและเปิดการใช้งาน
        skillButton.image.color = originalColor;
        skillButton.interactable = true;
    }

    private IEnumerator SelectLocationAndDamage()
    {
        bool locationSelected = false;

        while (!locationSelected)
        {
            if (Input.GetMouseButtonDown(0)) // คลิกซ้าย
            {
                Vector3 explosionPoint = Interaction.current.hit.point; // ตำแหน่งที่ชี้
                Collider[] colliders = Physics.OverlapSphere(explosionPoint, explosionRadius); // ตรวจสอบวัตถุในรัศมี
                Instantiate(effect, explosionPoint, Quaternion.identity);

                foreach (Collider nearbyObject in colliders)
                {
                    Enemy enemy = nearbyObject.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(skillDamage, damageType);
                    }
                }

                locationSelected = true;
            }

            if (Input.GetMouseButtonDown(1)) // คลิกขวา
            {
                CoinSystem.current.AddCoins(skillCost); // คืนค่าเหรียญเมื่อยกเลิก
                yield break; // ยกเลิก Coroutine
            }

            yield return null; // รอเฟรมถัดไป
        }
    }
}
