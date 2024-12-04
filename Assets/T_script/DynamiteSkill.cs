using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DynamiteSkill : MonoBehaviour
{

    public int skillCost = 100; // ราคาของสกิล
    public float skillDamage = 50; // ความเสียหายที่สกิลทำ
    public float cooldownTime = 30f; // เวลา Cooldown 30 วินาที
    public float explosionRadius = 5f; // รัศมีของการโจมตี AOE

    private bool isCooldown = false; // ตรวจสอบว่ากำลังอยู่ในช่วง Cooldown หรือไม่
    [SerializeField]private float cooldownTimer; // ตัวจับเวลาสำหรับ Cooldown

    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            

            if (cooldownTimer <= 0f)
            {
                isCooldown = false;       
            }
        }
    }

    public void ActivateSkill()
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

                    Vector3 explosionPoint = Interaction.current.hit.point; // ตำแหน่งที่ระเบิด
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
