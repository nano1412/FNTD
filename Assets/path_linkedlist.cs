using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class path_linkedlist : MonoBehaviour
{
    public GameObject nextPath;
    public List<GameObject> lastPath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextPath = BuildingSystem.current.humanKingdom;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AwaitForDestroy()
    {
        while (true)
        {
            if(transform.childCount <= 0)
            {
                nextPath.GetComponent<path_linkedlist>().AwaitForDestroy();
                Destroy(transform);
            }
        }
    }
}
