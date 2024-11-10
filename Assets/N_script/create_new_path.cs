using Unity.VisualScripting;
using UnityEngine;

public class create_new_path : MonoBehaviour
{
    public GameObject curser3DPrefab;
    private GameObject curser3D;
    public GameObject lastPath;
    private GameObject saveCurrentPath;
    [SerializeField] private GameObject pathPrefab;
    public GameObject spawner = null;
    [SerializeField] private GameObject createNewPathCanvas;
    private int pathcount;

    private bool isInCreateNewPathAction = false;

    private Vector3 offset;

    private void Start()
    {
        curser3D = Instantiate(curser3DPrefab, transform);

        curser3D.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + offset;
        curser3D.transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);

        if (Input.GetMouseButtonDown(0) && isInCreateNewPathAction && spawner != null)
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
}
