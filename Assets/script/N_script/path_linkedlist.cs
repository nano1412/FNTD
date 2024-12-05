using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class path_linkedlist : MonoBehaviour
{
    public GameObject enemys;
    public GameObject nextPath;
    public List<GameObject> lastPath;
    public List<GameObject> stone_walkway;
    public bool isFinishInstantiate = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextPath = BuildingSystem.current.humanKingdom;
        enemys = new GameObject("enemys");
        enemys.transform.parent = transform;
       
    }

    // Update is called once per frame
    void Update()
    {
        //path always have "flag" as a child
        if (isFinishInstantiate && lastPath.Count <= 0 && enemys.transform.childCount <= 0)
        {
                if(nextPath.tag == "path")
                {
                nextPath.GetComponent<path_linkedlist>().lastPath.Remove(gameObject);

                }
                Destroy(gameObject);
        }
    }

    private void AwaitForDestroy()
    {

    }
}
