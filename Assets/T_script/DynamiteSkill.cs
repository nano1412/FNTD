using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DynamiteSkill : MonoBehaviour
{
    public Button dynamiteButton; // ปุ่ม UI สำหรับใช้สกิล Dynamite
    public int skillCost = 100; // ราคาของสกิล
    public int skillDamage = 50; // ความเสียหายที่สกิลทำ
    public float cooldownTime = 30f; // เวลา Cooldown 30 วินาที
    public float explosionRadius = 5f; // รัศมีของการโจมตี AOE

    private Camera mainCamera;
    private bool isCooldown = false; // ตรวจสอบว่ากำลังอยู่ในช่วง Cooldown หรือไม่
    private float cooldownTimer; // ตัวจับเวลาสำหรับ Cooldown

    void Start()
    {
        mainCamera = Camera.main;

        if (dynamiteButton != null)
        {
            dynamiteButton.onClick.AddListener(ActivateSkill);
        }
    }

    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            dynamiteButton.interactable = false; // ปิดการใช้งานปุ่มในช่วง Cooldown

            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
                dynamiteButton.interactable = true; // เปิดการใช้งานปุ่มเมื่อ Cooldown สิ้นสุด
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
            Debug.Log("Skill Dynamite activated! Select a location.");
            StartCoroutine(SelectLocationAndDamage());
            StartCooldown(); // เริ่ม Cooldown
        }
        else
        {
            Debug.Log("Not enough coins to use Dynamite.");
        }
    }

    private void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = cooldownTime;
        Debug.Log($"Dynamite skill on cooldown for {cooldownTime} seconds.");
    }

    private IEnumerator SelectLocationAndDamage()
    {
        bool locationSelected = false;

        while (!locationSelected)
        {
            if (Input.GetMouseButtonDown(0)) // เมื่อคลิกซ้าย
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 explosionPoint = hit.point; // ตำแหน่งที่ระเบิด
                    Collider[] colliders = Physics.OverlapSphere(explosionPoint, explosionRadius); // ตรวจสอบวัตถุในรัศมี

                    foreach (Collider nearbyObject in colliders)
                    {
                        Enemy enemy = nearbyObject.GetComponent<Enemy>();
                        if (enemy != null)
                        {
                            enemy.TakeDamage(skillDamage);
                            Debug.Log($"{enemy.name} took {skillDamage} damage from Dynamite.");
                        }
                    }

                    Debug.Log($"Dynamite exploded at {explosionPoint} with radius {explosionRadius}.");
                    locationSelected = true;
                }
            }

            if (Input.GetMouseButtonDown(1)) // เมื่อคลิกขวา
            {
                Debug.Log("Skill Dynamite canceled.");
                CoinSystem.current.AddCoins(skillCost); // คืนค่าเหรียญเมื่อยกเลิก
                yield break; // ยกเลิก Coroutine
            }

            yield return null; // รอเฟรมถัดไป
        }
    }
}
