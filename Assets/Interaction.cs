using UnityEngine;

public class Interaction : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    [SerializeField] private GameObject createNewPathCanvas;

    [SerializeField] private GameObject saveHit;
    [SerializeField] private GameObject saveSpawner;
    [SerializeField] private create_new_path create_new_path_script;

    private void Start()
    {
        createNewPathCanvas.SetActive(false);
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                print(hit.collider.tag);
                saveHit = hit.collider.gameObject;
            }
        }

        if (saveHit.tag == "Spawner")
        {
            ActiveSpawnerCanvas();
            saveSpawner = saveHit.gameObject;
            saveHit = null;
        }
    }

    void ActiveSpawnerCanvas()
    {
        createNewPathCanvas.SetActive(true);
    }

    public void cancelSpawner()
    {
        createNewPathCanvas.SetActive(false);
    }

    public void ChangeSpawnerPath()
    {
        create_new_path_script.ChangePath(saveSpawner);
    }
}
