using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100; // Initial enemy health
    public int maxHP;
    [SerializeField] int coinReward;
    private bool killedByTurret = false; // Flag to check if the enemy was killed by a turret

    void Start()
    {
        maxHP = health;
        //Debug.Log("Enemy spawned with health: " + health);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        //Debug.Log("Enemy took damage, remaining health: " + health);

        if (health <= 0)
        {
            killedByTurret = true; // Mark as killed by turret
            Die();
        }
    }

    void Die()
    {
        //Debug.Log("Enemy died!");

        // Check if the enemy was killed by a turret
        if (killedByTurret)
        {
            CoinSystem.current.AddCoins(coinReward); // Call the CoinSystem to add coins when the enemy dies
            //Debug.Log("Coins added: 10");
        }

        Destroy(gameObject);
    }

    // Called when the enemy collides with the HumanKingdom
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Enemy collided with: " + other.gameObject.name); // Log the name of the collided object

        if (other.CompareTag("HumanKingdom")) // Check if the collision is with the HumanKingdom
        {
            PlayerStats.UpdateLives(1); // Reduce player's lives by 1
            //Debug.Log("Player loses 1 life. Remaining lives: " + PlayerStats.Lives);

            // Destroy the enemy without adding coins
            Destroy(gameObject);
        }
    }
}