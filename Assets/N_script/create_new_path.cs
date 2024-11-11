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
    private int pathcount;

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
            if(saveCurrentPath.tag == "path")
            {
                saveCurrentPath.GetComponent<path_linkedlist>().lastPath.Remove(spawner);
            }

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
            newpath.GetComponent<path_linkedlist>().lastPath.Add(lastPath);
        newpath.GetComponent<path_linkedlist>().isFinishInstantiate = true;
        lastPath = newpath;
    }

    public void NewChangePath()
    {
            Interaction.current.curser3D.SetActive(true);
        isAwaitingSelectSpawner = true;
    }
}
