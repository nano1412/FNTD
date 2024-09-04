using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] GameObject path;
    [SerializeField] GameObject enemyprefab;

    public int size = 1;
    float timer;
    public float spawnTimer;
    private int numOfPathChange = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        path = this.transform.Find("Path").gameObject;
        List<Transform> childrenList = new List<Transform>();
        childrenList.Add(transform);
        foreach (Transform child in path.transform)
        {
            // This iterates over all direct children, not the parent

            childrenList.Add(child);
        }

        points = childrenList.ToArray();

        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            for (int i = size; i > 0; i--)
            {
                Spawn();
            }
            timer = spawnTimer;
        }
    }

    void Spawn()
    {
        GameObject enemy = Instantiate(enemyprefab, this.transform.Find("Enemy"));
        enemy.GetComponent<path>().AddPath(points);
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
        path.name = "old path " + numOfPathChange;
        numOfPathChange++;

        points = childrenList.ToArray();
    }
}
