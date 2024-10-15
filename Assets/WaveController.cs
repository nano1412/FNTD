using UnityEngine;
using TMPro;
using System.Drawing;
using UnityEditor.Playables;

public class WaveController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private int wave = 1;
    [SerializeField] private int enemiesInWave = 10;

    [SerializeField] private GameObject[] spawnerPrefab;
    [SerializeField] private GameObject[] towerPrefab;

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

            //enemy formula
            enemiesInWave = 10 + (wave * 2);

            if (wave % 5 == 0)
            {
                //up map size formula
                BuildingSystem.current.buildingRange = 250 * (wave / 5);
            }
        }
    }

    private void DistrubuteEnemies()
    {
        numSendToSpawner = Random.Range(0, enemiesInWave);
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

    public void RNGBuilding( int turretAmount)
    {
        //spawner
        while (turretAmount > 0)
        {
            Vector3 randomPosition = new Vector3(Random.Range(BuildingSystem.current.noRNGSpawnRange, BuildingSystem.current.buildingRange) * RandomSign(), 0.6f, Random.Range(BuildingSystem.current.noRNGSpawnRange, BuildingSystem.current.buildingRange) * RandomSign());

            if (BuildingSystem.current.InitializeObjectRNG(towerPrefab[0], randomPosition))
            {
                turretAmount--;
            }
        }

        //turret
        while (false)
        {
            Vector3 randomPosition = new Vector3(Random.Range(BuildingSystem.current.noRNGSpawnRange, BuildingSystem.current.buildingRange) * RandomSign(), 0.6f, Random.Range(BuildingSystem.current.noRNGSpawnRange, BuildingSystem.current.buildingRange) * RandomSign());

            if (BuildingSystem.current.InitializeObjectRNG(towerPrefab[0], randomPosition)) {
                turretAmount--;
            }
        }
    }

    public static int RandomSign()
    {
        return Random.value < 0.5f ? 1 : -1;
    }
}
