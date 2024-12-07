using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;
using UnityEditor;
using System.Diagnostics.CodeAnalysis;

public class WaveController : MonoBehaviour
{
    public static WaveController current;

    [SerializeField] private TMP_Text waveText;
    [SerializeField] private int wave = 1;
    [SerializeField] private int enemiesInWave = 10;

    [SerializeField] private GameObject[] spawnerPrefab;
    [SerializeField] private GameObject[] towerPrefab;
    [SerializeField] private GameObject[] environmentPrefab;

    public GameObject spawners;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float hpMultiplier = 1f;
    [SerializeField] private float spawnTimerMultiplier = 1f;
    [SerializeField] private float CoinRewardMultiplier = 1f;
    public float CoinRewardbase = 1f;

    [SerializeField] private EnemyWithWeight[] commonEnemiesPrefab;
    [SerializeField] private EnemyWithWeight[] rareEnemiesPrefab;
    [SerializeField] private EnemyWithWeight[] bossEnemiesPrefab;

    [SerializeField] private int commonEnemiesWeight;
    [SerializeField] private int rareEnemiesWeight;
    [SerializeField] private int bossEnemiesWeight;

    private int maxCommonEnemiesWeight;
    private int maxRareEnemiesWeight;
    private int maxBossEnemiesWeight;

    [SerializeField] private int numToSpawnenvironment;
    [SerializeField] private int numToSpawnSpawner;
    [SerializeField] private int numToSpawnTower;

    private int numToSpawnenvironmentPrefab;
    private int numToSpawnSpawnerPrefab;
    private int numToSpawnTowerPrefab;

    [SerializeField] private GameObject allspawners;

    float nextWaveTimer;
    public float spawnTimer;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        //UpdateWeight();
        //DistrubuteEnemies();
    }

    void Update()
    {
        //time interval so it wont go too fast
        nextWaveTimer -= Time.deltaTime;
        if (nextWaveTimer < 0f)
        {

            CheckWave();
            nextWaveTimer = spawnTimer;
        }

        if (enemiesInWave < 0) { enemiesInWave = 0; }
    }

    private void CheckWave()
    {
        waveText.text = "Wave: " + wave;

        if (!(IsEnemiesLeft() + enemiesInWave > 0))
        {
            wave++;
            if (wave % 5 == 0)
            {
                //up map size formula
                BuildingSystem.current.buildingRange = 250 * ((wave / 5) + 2);

                numToSpawnenvironmentPrefab = numToSpawnenvironment;
                BuildingSystem.current.RNGBuilding(numToSpawnenvironmentPrefab, environmentPrefab);
                numToSpawnenvironmentPrefab = 0;
            }

            if (wave % 10 == 0)
            {
                for (int i = wave / 10; i > 0; i--)
                {
                    selectEnemyInRarity(bossEnemiesPrefab, maxBossEnemiesWeight);
                }
            }

            //Multiplier spawn rate/hp/speed
            foreach (Transform spawner in spawners.transform)
            {
                if (spawner.CompareTag("Spawner"))
                {
                    spawner.GetComponent<Spawner>().speedMultiplier *= speedMultiplier;
                    spawner.GetComponent<Spawner>().hpMultiplier *= hpMultiplier;
                    spawner.GetComponent<Spawner>().spawnTimer *= spawnTimerMultiplier;
                }
            }
            CoinRewardbase = CoinRewardbase * CoinRewardMultiplier;

            //number to spawn prefab formula
            numToSpawnSpawnerPrefab = numToSpawnSpawner;
            numToSpawnTowerPrefab = numToSpawnTower;

            BuildingSystem.current.RNGBuilding(numToSpawnSpawnerPrefab, spawnerPrefab);
            BuildingSystem.current.RNGBuilding(numToSpawnTowerPrefab, towerPrefab);

            //dont want to pass by ref. but want to reset the value after it's done execute
            numToSpawnSpawnerPrefab = 0;
            numToSpawnTowerPrefab = 0;


            //enemy formula
            enemiesInWave = 10 + (wave * 2);

        }
            UpdateWeight();
            DistrubuteEnemies();
    }

    void UpdateWeight()
    {
        maxCommonEnemiesWeight = 0;
        maxRareEnemiesWeight = 0;
        maxBossEnemiesWeight = 0;

        foreach (EnemyWithWeight i in commonEnemiesPrefab)
        {
            maxCommonEnemiesWeight += i.weight;
        }

        foreach (EnemyWithWeight i in rareEnemiesPrefab)
        {
            maxRareEnemiesWeight += i.weight;
        }

        foreach (EnemyWithWeight i in bossEnemiesPrefab)
        {
            maxBossEnemiesWeight += i.weight;
        }
    }

    private void DistrubuteEnemies()
    {
        while (enemiesInWave > 0)
        {
            int enemyRarity = Random.Range(0, commonEnemiesWeight + rareEnemiesWeight + bossEnemiesWeight);
            switch (enemyRarity)
            {
                case int i when (i < commonEnemiesWeight):
                    //Debug.Log("is common");
                    selectEnemyInRarity(commonEnemiesPrefab, maxCommonEnemiesWeight);
                    break;

                case int i when (i < rareEnemiesWeight):
                    //Debug.Log("is rare");
                    selectEnemyInRarity(rareEnemiesPrefab, maxRareEnemiesWeight);
                    break;

                case int i when (i < bossEnemiesWeight):
                    //Debug.Log("is boss");
                    selectEnemyInRarity(bossEnemiesPrefab, maxBossEnemiesWeight);
                    break;
            }
            enemiesInWave--;
        }
    }

    void selectEnemyInRarity(EnemyWithWeight[] enemywithweight, int maxWeight)
    {
        int randomWeight = Random.Range(0, maxWeight);
        for (int i = 0; i < enemywithweight.Length; i++)
        {
            randomWeight -= enemywithweight[i].weight;
            if (randomWeight < 0)
            {
                //Debug.Log("enemy no " + i);
                SendEnemiesToSpawner(enemywithweight[i].enemy);
                return;
            }
        }
        //Debug.Log("edge go with 1");
        SendEnemiesToSpawner(enemywithweight[0].enemy);
        return;
    }
    private bool SendEnemiesToSpawner(GameObject enemy)
    {
        int randomChildIndex = Random.Range(0, allspawners.transform.childCount);
        GameObject spawner = allspawners.transform.GetChild(randomChildIndex).gameObject;
        if (spawner.GetComponent<Spawner>() == null)
        {
            Debug.Log("this is not spawner");
            return false;
        }
        //Debug.Log("spawner no " + randomChildIndex);
        spawner.GetComponent<Spawner>().AddToSpawnQueue(enemy);
        return true;
    }

    private int IsEnemiesLeft()
    {
        int enemiesLeftInmap = 0;
        int enemiesLeftToBeSpawn = 0;

        foreach (Transform spawner in allspawners.transform)
        {
            if (spawner.GetComponent<Spawner>() != null)
            {
                enemiesLeftToBeSpawn += spawner.GetComponent<Spawner>().enemiesList.Count;
                enemiesLeftInmap += spawner.Find("Enemy").transform.childCount;
            }
        }

        return enemiesLeftInmap + enemiesLeftToBeSpawn;
    }
    public void ResetWave()
    {
        wave = 1;
        enemiesInWave = 10; // ���絨ӹǹ�ѵ��� wave �á
        UpdateWeight();
        DistrubuteEnemies();
        waveText.text = "Wave: " + wave; // �ѻവ��ͤ����ʴ� Wave
    }
}
