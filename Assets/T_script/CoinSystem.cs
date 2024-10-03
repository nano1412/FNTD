using UnityEngine;
using TMPro;

public class CoinSystem : MonoBehaviour
{
    public static int Coins; // จำนวนเหรียญปัจจุบัน
    public int startCoins = 1000; // จำนวนเหรียญเริ่มต้น

    public TMP_Text coinText; // ตัวแสดงเหรียญใน UI

    void Start()
    {
        Coins = startCoins; // กำหนดจำนวนเหรียญเริ่มต้น
        Debug.Log("Game started with " + Coins + " coins."); // แสดงผลใน Console
        UpdateCoinText(); // อัพเดท UI ของเหรียญ
    }

    // ฟังก์ชันสำหรับการหักเหรียญ
    public static bool SpendCoins(int amount)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
            Debug.Log("Coins spent: " + amount + ". Current coins: " + Coins);
            return true;
        }
        else
        {
            Debug.Log("Not enough coins. Current coins: " + Coins);
            return false;
        }
    }

    // ฟังก์ชันสำหรับการเพิ่มเหรียญ
    public static void AddCoins(int amount)
    {
        Coins += amount;
        Debug.Log("Coins added: " + amount + ". Current coins: " + Coins);
    }

    // อัพเดท UI ของเหรียญ
    private void Update()
    {
        UpdateCoinText();
    }

    // แสดงจำนวนเหรียญบนหน้าจอ
    private void UpdateCoinText()
    {
        coinText.text = "Coins: " + Coins;
    }
}