using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Lives;
    public int startLives = 5;

    public TMP_Text hpText;

    void Start()
    {
        Lives = startLives;
        //Debug.Log("Game started with " + Lives + " lives.");

        hpText.text = "HP: " + Lives;
    }

    // Method for updating player lives and logging
    public static void UpdateLives(int amount)
    {
        Lives -= amount;

        // Ensure lives do not drop below zero
        if (Lives < 0)
        {
            Lives = 0;
            //Debug.Log("Lives updated. Current lives: " + Lives + ". Game Over.");
        }
        else if (amount < 0)
        {
            //Debug.Log("Player loses " + Mathf.Abs(amount) + " life(s). Remaining lives: " + Lives);
        }
    }

    private void Update()
    {
        // Update displayed HP
        if (Lives <= 0)
        {
            hpText.text = "Game over HP: " + Lives;
        }
        else
        {
            hpText.text = "HP: " + Lives;
        }
    }
}