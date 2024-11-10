using UnityEngine;

public class path_linkedlist : MonoBehaviour
{
    public GameObject nextPath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextPath = BuildingSystem.current.humanKingdom;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
