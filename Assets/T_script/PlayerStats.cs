using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Lives;
    public int startLives = 20;

    public TMP_Text hpText;

    void Start()
    {
        Lives = startLives;
        Debug.Log("Game started with " + Lives + " lives.");

        hpText.text = "HP: " + Lives;
    }

    // เมธอดสำหรับอัพเดตชีวิตและแสดง Log
    public static void UpdateLives(int amount)
    {
        Lives -= amount;

        if (amount > 0)
        {
            Debug.Log("Lives decreased by " + Mathf.Abs(amount) + ". Current lives: " + Lives);
        }

        // ตรวจสอบหากชีวิตลดลงเหลือน้อยกว่า 0
        if (Lives <= 0)
        {
            Lives = 0;
            Debug.Log("Lives updated. Current lives: " + Lives + ". Game Over.");
        }
        else if (amount > 0)
        {
            Debug.Log("Lives increased by " + amount + ". Current lives: " + Lives);
        }

        
    }

    private void Update()
    {
        

        if(Lives <= 0)
        {
            hpText.text = "Game over HP: " + Lives;
        } else
        {
            hpText.text = "HP: " + Lives;
        }
    }
}