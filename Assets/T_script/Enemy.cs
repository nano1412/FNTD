using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 40; // ตั้งค่าเลือดเริ่มต้นเป็น 40
    private bool killedByTurret = false; // ตัวแปรเพื่อเช็คว่าศัตรูถูกป้อมยิงตายหรือไม่
    public int maxHP;

    void Start()
    {
        maxHP = health;
        Debug.Log("Enemy spawned with health: " + health);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Enemy took damage, remaining health: " + health);

        if (health <= 0)
        {
            killedByTurret = true; // ศัตรูถูกป้อมยิงตาย
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        // เช็คว่าศัตรูถูกยิงตาย หากใช่ให้เพิ่มเหรียญ
        if (killedByTurret)
        {
            PlayerStats.AddCoins(10); // เพิ่มเหรียญ 10 เมื่อศัตรูตายจากการโดนยิง
            Debug.Log("Coins added: 10");
        }

        Destroy(gameObject);
    }

    // ฟังก์ชันที่ถูกเรียกใช้เมื่อศัตรูชนกับ HumanKingdom
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enemy collided with: " + other.gameObject.name); // Log แสดงชื่อของวัตถุที่ชน

        if (other.CompareTag("HumanKingdom")) // ตรวจสอบแท็กของวัตถุที่ชน
        {
            PlayerStats.UpdateLives(1); // ลดชีวิตของผู้เล่นลง 1
            Debug.Log("Player loses 1 life. Remaining lives: " + PlayerStats.Lives);

            // ลบศัตรูออกโดยไม่ให้เพิ่มเหรียญ
            Destroy(gameObject); // ทำลายศัตรูเมื่อชนกับ HumanKingdom
        }
    }
}