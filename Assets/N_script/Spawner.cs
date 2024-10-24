using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] GameObject path;
    public List<GameObject> enemiesList;
    [SerializeField] string humanKingdomName;
    [SerializeField] string spawners;
    [SerializeField] GameObject humanKingdom;

    float timer;
    public float spawnTimer;
    private int numOfPathChange = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.parent = GameObject.Find(spawners).transform;

        humanKingdom = GameObject.Find(humanKingdomName);
        path = this.transform.Find("Path").gameObject;
        List<Transform> childrenList = new List<Transform>();
        childrenList.Add(transform);
        foreach (Transform child in path.transform)
        {
            // This iterates over all direct children, not the parent

            childrenList.Add(child);
        }
        childrenList.Add(humanKingdom.transform);

        points = childrenList.ToArray();
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
        if(enemiesList.Count <= 0) { return; }

        if (enemiesList[0].tag == "Enemy")
        {
            Debug.Log("is enemy");
            GameObject enemy = Instantiate(enemiesList[0], this.transform.Find("Enemy"));
            enemy.GetComponent<path>().AddPath(points);
        }
        enemiesList.RemoveAt(0);
    }

    public void ChangePath(string newPathName)
    {
        timer = spawnTimer;
        path = this.transform.Find(newPathName).gameObject;

        List<Transform> childrenList = new List<Transform>();
        childrenList.Add(transform);
        foreach (Transform child in path.transform)
        {
            // This iterates over all direct children, not the parent

            childrenList.Add(child);
        }
        childrenList.Add(humanKingdom.transform);
        path.name = "Path " + numOfPathChange;
        numOfPathChange++;

        points = childrenList.ToArray();
    }
}
