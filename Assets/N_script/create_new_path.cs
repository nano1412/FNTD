using Unity.VisualScripting;
using UnityEngine;

public class create_new_path : MonoBehaviour
{
    public GameObject curser3DPrefab;
    private GameObject curser3D;
    public GameObject lastPath;
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
            //place path
            GameObject newpath = Instantiate(pathPrefab, curser3D.transform.position, new Quaternion(), BuildingSystem.current.paths.transform);


            if( lastPath.tag == "Spawner")
            {
                lastPath.GetComponent<Spawner>().nextPath = newpath;
            } else if (lastPath.tag == "path")
            {
                lastPath.GetComponent<path_linkedlist>().nextPath = newpath;
            }
            else
            {
                Debug.Log("invalid path placement");
                Destroy(transform);
            }

            lastPath = newpath;
            pathcount++;
        }

        //temporary hard stop create new path with maximun of 5 ToPath
        //new path finish
        if(pathcount >= 5)
        {
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
        lastPath = spawner;
        isInCreateNewPathAction = true;
        curser3D.SetActive(true);
    }
}
