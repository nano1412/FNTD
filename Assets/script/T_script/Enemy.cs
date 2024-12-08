using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using static DamageType;


public enum DamageType
{
    physical,
    magic
}

public enum Rarity
{
    common,
    rare,
    boss
}


public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float health = 100f; // Initial enemy health
    public float physicalResistance;
    public float magicResistance;
    public float maxHP;
    public float moveSpeed = 5f;
    [SerializeField] Rarity rarity;
    [SerializeField] int coinReward;
    private bool isDieByHP = false; // Flag to check if the enemy was killed by a turret

    public GameObject HPbarPrefab;
    GameObject HPbar;
    public Image healthbarFill;
    public float healthbarOffset;

    public GameObject physicalDamageEffect;
    public GameObject magicDamageEffect;
    public GameObject deadEffect;

    void Start()
    {
       HPbar = Instantiate(HPbarPrefab, Vector3.zero,Quaternion.identity, transform);
        HPbar.GetComponent<RectTransform>().localPosition = new Vector3(0, healthbarOffset,0);
        healthbarFill = HPbar.transform.Find("fill").GetComponent<Image>();

        maxHP = health;
        //Debug.Log("Enemy spawned with health: " + health);

    }

    private void Update()
    {
        //make hpbar look on camera
        HPbar.transform.LookAt(2 * HPbar.transform.position - Camera.main.transform.position);
    }


    public void TakeDamage(float amount, DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.physical:
                Instantiate(physicalDamageEffect, transform);
                amount = amount * (100 - physicalResistance) / 100;
                break;
            case DamageType.magic:
                Instantiate(magicDamageEffect, transform);
                amount = amount * (100 - magicResistance) / 100;
                break;
        }
        health -= amount;

        UpdateHPbar(health/maxHP);
        //Debug.Log("Enemy took damage, remaining health: " + health);

        if (health <= 0)
        {
            isDieByHP = true; // Mark as killed by turret
            AddKill();
            Die();
        }
    }

    public void UpdateHPbar(float amount)
    {
        healthbarFill.fillAmount = amount;
    }

    private void AddKill()
    {
        switch (rarity)
        {
            case Rarity.common:
                ScoreController.current.AddCommonKill();
                break;
            case Rarity.rare:
                ScoreController.current.AddRareKill();
                break;
            case Rarity.boss:
                ScoreController.current.AddBossKill();
                break;
        }
    }

    void Die()
    {
        //Debug.Log("Enemy died!");

        // Check if the enemy was killed by a turret
        if (isDieByHP)
        {
            CoinSystem.current.AddCoins(coinReward); // Call the CoinSystem to add coins when the enemy dies
            //Debug.Log("Coins added: 10");
            Instantiate(deadEffect,transform);
        }

        Destroy(gameObject,0.5f);
    }

    // Called when the enemy collides with the HumanKingdom
    void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("HumanKingdom")) // Check if the collision is with the HumanKingdom
        {
            PlayerStats.current.UpdateLives(1); // Reduce player's lives by 1
            //Debug.Log("Player loses 1 life. Remaining lives: " + PlayerStats.Lives);

            // Destroy the enemy without adding coins
            Destroy(gameObject);
        }
    }
}