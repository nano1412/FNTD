using TMPro;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    public static int Coins;
    public int startCoins = 1000;

    public TMP_Text coinText;

    void Start()
    {
        Coins = startCoins; // Initialize coins at the start of the game
        Debug.Log("Game started with " + Coins + " coins.");

        coinText.text = "Coins: " + Coins; // Display starting coin count
    }

    // Method for updating coins and logging
    public static void AddCoins(int amount)
    {
        Coins += amount;
        Debug.Log("Coins updated. Current coins: " + Coins);
    }

    private void Update()
    {
        // Update displayed coin count
        coinText.text = "Coins: " + Coins;
    }
}
