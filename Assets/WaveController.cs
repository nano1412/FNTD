using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class WaveController : MonoBehaviour
{
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
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

            RNGBuilding(numToSpawnSpawnerPrefab, spawnerPrefab);
            RNGBuilding(numToSpawnTowerPrefab, towerPrefab);

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

    public void RNGBuilding(int ObjectAmount, GameObject[] gameObjectPrefab)
    {
        int attempt = 0;
        //spawner
        while (ObjectAmount > 0)
        {
            Vector3 randomPosition = new Vector3(Random.Range(0, BuildingSystem.current.buildingRange) * RandomSign(), 0.6f, Random.Range(0, BuildingSystem.current.buildingRange) * RandomSign());
            Vector3 randomPositionSnap = BuildingSystem.current.SnapCoordinateToGrid(randomPosition);

            bool isInValidSpace = (IsInSquare(BuildingSystem.current.buildingRange, randomPositionSnap.x, randomPositionSnap.z)
                               && !IsInSquare(BuildingSystem.current.noRNGSpawnRange, randomPositionSnap.x, randomPositionSnap.z));


            if (isInValidSpace)
            {
                try
                {
                    if (BuildingSystem.current.InitializeObjectRNG(gameObjectPrefab[Random.Range(0,gameObjectPrefab.Length)], randomPositionSnap))
                    {
                        ObjectAmount--;
                    }
                    else
                    {
                        attempt++;
                    }
                }
                catch(IndexOutOfRangeException ex)
                {
                    //Debug.LogError(ex.Message);
                    attempt++;
                }
            }

            if(attempt > 20)
            {
                Debug.LogError("attempt to spawn Enemies spawner are excess 20, stop the function");
                break;
            }
        }
    }

    private static int RandomSign()
    {
        return Random.value < 0.5f ? 1 : -1;
    }

    private bool IsInSquare(float range,float x, float y)
    {
        bool isXInRange = (x <= range && x >= -range);
        bool isYInRange = (y <= range && y >= -range);

        return isXInRange && isYInRange;
    }
}
