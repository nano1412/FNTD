using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class create_new_path : MonoBehaviour
{
    [SerializeField] private int skillCost;
    public GameObject lastPath;
    public GameObject curser3D;
    private GameObject saveCurrentPath;
    [SerializeField] private GameObject pathPrefab;
    public GameObject spawner = null;
    [SerializeField] private GameObject createNewPathCanvas;
    [SerializeField] private float rockDistance = 10f;
    public List<GameObject> gameobject_stone_walkway;
    private int pathcount;
    private bool isEndOnMerge = false;

    private bool isInCreateNewPathAction = false;

    //for new button function
    private bool isAwaitingSelectSpawner = false;

    private void Start()
    {
        curser3D = Interaction.current.curser3D;
    }


    // Update is called once per frame
    void Update()
    {
        //for new button function
        if (Input.GetMouseButtonDown(0) && isAwaitingSelectSpawner)
        {
            if (Interaction.current.saveHit.tag == "Spawner" && CoinSystem.current.SpendCoins(skillCost))
            {
                ChangePath(Interaction.current.saveHit);
            }
            else
            {
                Interaction.current.curser3D.SetActive(false);
            }
            isAwaitingSelectSpawner = false;
        }

        if (Input.GetMouseButtonDown(0) && isInCreateNewPathAction && spawner != null && Interaction.current.saveHit.tag != "Spawner")
        {
            //use same path as other
            if (Interaction.current.saveHit.tag == "path")
            {
                GameObject mergePath = Interaction.current.saveHit as GameObject;

                AddNewPath(mergePath);

                pathcount = 5;
                isEndOnMerge = true;
            }
            //create new path
            else
            {
                GameObject newpath = Instantiate(pathPrefab, curser3D.transform.position, new Quaternion(), BuildingSystem.current.paths.transform);

                AddNewPath(newpath);
                pathcount++;
            }  
        }

        //temporary hard stop create new path with maximun of 5 ToPath
        //new path finish
        if(pathcount >= 5)
        {
            if (!isEndOnMerge)
            {
                WalkStone(lastPath, BuildingSystem.current.humanKingdom);
            }
            if (saveCurrentPath.tag == "path")
            {
                saveCurrentPath.GetComponent<path_linkedlist>().lastPath.Remove(spawner);
            }
            isEndOnMerge = false;
            pathcount = 0;
            spawner = null;
            isInCreateNewPathAction = false;
            curser3D.SetActive(false);
            createNewPathCanvas.SetActive(false);
        }
    }

    public void ChangePath(GameObject spawner)
    {
        pathcount = 0;
        this.spawner = spawner;
        saveCurrentPath = spawner.GetComponent<Spawner>().nextPath;
        lastPath = spawner;
        isInCreateNewPathAction = true;
        curser3D.SetActive(true);
    }

    private void AddNewPath(GameObject newpath)
    {
            if (lastPath.tag == "Spawner")
            {
                lastPath.GetComponent<Spawner>().nextPath = newpath;
            }
            else if (lastPath.tag == "path")
            {
                lastPath.GetComponent<path_linkedlist>().nextPath = newpath;
            }
            else
            {
                Debug.Log("invalid path placement");
                Destroy(transform);
            }
        WalkStone(lastPath,newpath);
            newpath.GetComponent<path_linkedlist>().lastPath.Add(lastPath);
        newpath.GetComponent<path_linkedlist>().isFinishInstantiate = true;
        lastPath = newpath;
    }

    public void NewChangePath()
    {
            Interaction.current.curser3D.SetActive(true);
        isAwaitingSelectSpawner = true;
    }

    public void WalkStone(GameObject lastPath, GameObject newpath)
    {
        Vector3 thisPostition = newpath.transform.position;
        Vector3 lastposition = lastPath.transform.position;

        for(float distance = rockDistance; distance <= Vector3.Distance(lastPath.transform.position, newpath.transform.position); distance += rockDistance)
        {
            Vector3 tempStonePostition = LerpByDistance(lastposition, thisPostition, distance);
            tempStonePostition.y = 0;
            int randomNum = Random.Range(0, 3);
            GameObject tempStone = Instantiate(gameobject_stone_walkway[randomNum], tempStonePostition, Quaternion.Euler(0,Random.Range(0, 360),0));

            if (lastPath.tag == "path")
            {
                tempStone.transform.parent = lastPath.transform;
            }
            else
            {
                tempStone.transform.parent = newpath.transform;
            }
        }
        
    }
    
    //example code
    public Vector3 LerpByDistance(Vector3 A, Vector3 B, float x)
    {
        Vector3 P = x * Vector3.Normalize(B - A) + A;
        return P;
    }
}
