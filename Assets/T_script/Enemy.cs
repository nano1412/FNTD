using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 40; // ตั้งค่าเลือดเริ่มต้นเป็น 40
    public int MaxHP;

    void Start()
    {
        Debug.Log("Enemy spawned with health: " + health);
        MaxHP = health;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Enemy took damage, remaining health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }

    // ฟังก์ชันที่ถูกเรียกใช้เมื่อศัตรูชนกับ HumanKingdom
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enemy collided with: " + other.gameObject.name); // Log แสดงชื่อของวัตถุที่ชน

        if (other.CompareTag("HumanKingdom")) // ตรวจสอบแท็กของวัตถุที่ชน
        {
            /*Debug.Log("Enemy reached HumanKingdom!"); // ตรวจสอบว่าชน HumanKingdom
            PlayerStats.Lives -= 1; // ลดชีวิตของผู้เล่นลง 1
            Debug.Log("Player loses 1 life. Remaining lives: " + PlayerStats.Lives);*/

            PlayerStats.UpdateLives(1);

            Destroy(gameObject); // ทำลายศัตรูเมื่อชนกับ HumanKingdom
        }
    }
}
