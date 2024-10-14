using UnityEngine;
using TMPro;
using System.Drawing;

public class WaveController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private int wave = 1;
    [SerializeField] private int enemiesInWave = 10;
    private int numSendToSpawner;
    private int spawnerIndex = 1;
    [SerializeField] private GameObject spawners;

    float nextWaveTimer;
    public float spawnTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this will distrubute the enemiesInWave to all spawner
        numSendToSpawner = Random.Range(0, enemiesInWave);
        Debug.Log(numSendToSpawner);
        while(enemiesInWave > 0)
        {
            spawnerIndex = Random.Range(0,spawners.transform.childCount - 1);

            if(SendEnemiesToSpawner(spawners.transform.GetChild(spawnerIndex).gameObject, numSendToSpawner))
            {
                enemiesInWave -= numSendToSpawner;

            }
        }

        //this will be timer for next wave
        nextWaveTimer -= Time.deltaTime;
        if (nextWaveTimer < 0f)
        {
            nextWaveTimer = spawnTimer;
        }

        if (enemiesInWave < 0) { enemiesInWave = 0; }
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
