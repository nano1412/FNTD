using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;
using UnityEditor;

public class WaveController : MonoBehaviour
{
    public static WaveController current;

    [SerializeField] private TMP_Text waveText;
    [SerializeField] private int wave = 1;
    [SerializeField] private int enemiesInWave = 10;

    [SerializeField] private GameObject[] spawnerPrefab;
    [SerializeField] private GameObject[] towerPrefab;

    [SerializeField] private int numToSpawnSpawner;
    [SerializeField] private int numToSpawnTower;

    private int numToSpawnSpawnerPrefab;
    private int numToSpawnTowerPrefab;

    private int numSendToSpawner;
    private int spawnerIndex = 1;
    
    [SerializeField] private GameObject spawners;

    float nextWaveTimer;
    public float spawnTimer;

    private void Awake()
    {
        current = this;
    }

    void Update()
    {
        //time interval so it wont go too fast
        nextWaveTimer -= Time.deltaTime;
        if (nextWaveTimer < 0f)
        {
            DistrubuteEnemies();
            CheckWave();
            nextWaveTimer = spawnTimer;
        }

        if (enemiesInWave < 0) { enemiesInWave = 0; }
    }

    private void CheckWave()
    {
        waveText.text = "Wave: " + wave;

        if (!(IsEnemiesLeft() > 0))
        {
            wave++;

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

            if (wave % 5 == 0)
            {
                //up map size formula
                BuildingSystem.current.buildingRange = 250 * ((wave / 5)+1);
            }
        }
    }

    private void DistrubuteEnemies()
    {
        numSendToSpawner = 1;
        while (enemiesInWave > 0)
        {
            spawnerIndex = Random.Range(0, spawners.transform.childCount - 1);

            if (SendEnemiesToSpawner(spawners.transform.GetChild(spawnerIndex).gameObject, numSendToSpawner))
            {
                if(numSendToSpawner > enemiesInWave) numSendToSpawner = enemiesInWave;
                enemiesInWave -= numSendToSpawner;

            }
        }
    }

    private int IsEnemiesLeft()
    {
        int enemiesLeftInmap = 0;
        int enemiesLeftToBeSpawn = 0;

        foreach(Transform spawner in spawners.transform)
        {
            if (spawner.GetComponent<Spawner>() != null)
            {
                enemiesLeftToBeSpawn += spawner.GetComponent<Spawner>().numToSpawn;
                enemiesLeftInmap += spawner.Find("Enemy").transform.childCount;
            }
        }

        return enemiesLeftInmap + enemiesLeftToBeSpawn;
    }

    private bool SendEnemiesToSpawner(GameObject spawner, int numToSpawn)
    {
        if(spawner.GetComponent<Spawner>() == null)
        {
            Debug.Log("this is not spawner");
            return false;
        }

        spawner.GetComponent<Spawner>().numToSpawn += numToSpawn;
        return true;
    }
}
