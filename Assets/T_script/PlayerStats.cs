using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Lives;
    public int startLives = 20;

    public static int Coins; // ตัวแปรสำหรับเก็บจำนวนเหรียญ
    public int startCoins = 1000;

    public TMP_Text hpText;
    public TMP_Text coinText; // เพิ่ม Text สำหรับแสดงจำนวนเหรียญ

    void Start()
    {
        Lives = startLives;
        Coins = startCoins; // ตั้งค่าเริ่มต้นของเหรียญ
        Debug.Log("Game started with " + Lives + " lives and " + Coins + " coins.");

        hpText.text = "HP: " + Lives;
        coinText.text = "Coins: " + Coins; // แสดงจำนวนเหรียญเริ่มต้น
    }

    // เมธอดสำหรับอัพเดตชีวิตและแสดง Log
    public static void UpdateLives(int amount)
    {
        Lives -= amount;

        // ตรวจสอบหากชีวิตลดลงเหลือน้อยกว่า 0
        if (Lives < 0)
        {
            Lives = 0;
            Debug.Log("Lives updated. Current lives: " + Lives + ". Game Over.");
        }
        else if (amount < 0)
        {
            Debug.Log("Player loses " + Mathf.Abs(amount) + " life(s). Remaining lives: " + Lives);
        }
    }

    // เมธอดสำหรับอัพเดตเหรียญและแสดง Log
    public static void AddCoins(int amount)
    {
        Coins += amount;
        Debug.Log("Coins updated. Current coins: " + Coins);
    }

    private void Update()
    {
        // อัปเดตข้อความสำหรับแสดง HP
        if (Lives <= 0)
        {
            hpText.text = "Game over HP: " + Lives;
        }
        else
        {
            hpText.text = "HP: " + Lives;
        }

        // อัปเดตข้อความสำหรับแสดงจำนวนเหรียญ
        coinText.text = "Coins: " + Coins;
    }
}