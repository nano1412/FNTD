using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    public GameObject nextPath;
    public List<GameObject> enemiesList;
    [SerializeField] string spawners;

    public float speedMultiplier = 1f;
    public float hpMultiplier = 1f;
    float timer;
    public float spawnTimer;
    private int numOfPathChange = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.parent = GameObject.Find(spawners).transform;
        nextPath = BuildingSystem.current.humanKingdom;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            Spawn();
            timer = spawnTimer;
        }
    }

    public void AddToSpawnQueue(GameObject enemy)
    {
        enemiesList.Add(enemy);
    }

    void Spawn()
    {
        if (enemiesList.Count <= 0) { return; }

        if (enemiesList[0].tag == "Enemy")
        {
            //Debug.Log("is enemy");
            GameObject enemy = Instantiate(enemiesList[0], this.transform.Find("Enemy"));
            enemy.GetComponent<Enemy>().health *= hpMultiplier;
            enemy.GetComponent<path>().moveSpeed *= speedMultiplier;
            Debug.Log(enemy.GetComponent<Enemy>().health *= hpMultiplier);
            enemy.GetComponent<path>().ToPath = nextPath;
        }
        enemiesList.RemoveAt(0);
    }
}
