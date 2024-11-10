using Unity.VisualScripting;
using UnityEngine;

public class create_new_path : MonoBehaviour
{
    public GameObject curser3DPrefab;
    private GameObject curser3D;
    public GameObject tempPath;
    [SerializeField] private GameObject pathPrefab;
    public GameObject spawner = null;
    [SerializeField] private GameObject createNewPathCanvas;

    private bool isInCreateNewPathAction = false;

    private Vector3 offset;

    private void Start()
    {
        curser3D = Instantiate(curser3DPrefab, transform);
        tempPath = Instantiate(new GameObject("tempPaths"), transform);

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
            GameObject path = Instantiate(pathPrefab, curser3D.transform.position, new Quaternion(), tempPath.transform);
        }

        //temporary hard stop create new path with maximun of 5 points
        //new path finish
        if(tempPath.transform.childCount >= 5)
        {
            string newPathName = tempPath.name;
            tempPath.transform.SetParent(spawner.transform);
            spawner.GetComponent<Spawner>().ChangePath(newPathName);

            tempPath = Instantiate(new GameObject("tempPaths"), transform);
            spawner = null;
            isInCreateNewPathAction = false;
            curser3D.SetActive(false);
            createNewPathCanvas.SetActive(false);
        }
    }

    public void ChangePath(GameObject spawner)
    {
        this.spawner = spawner;
        isInCreateNewPathAction = true;
        curser3D.SetActive(true);
    }
}
