using UnityEngine;
using TMPro;

public class Building_Button : MonoBehaviour
{
    [SerializeField] private GameObject gameController;
    [SerializeField] private BuildingSystem buildingSystem;
    [SerializeField] private int buttonIndex;
    [SerializeField] private GameObject objectToBuild;

    [SerializeField] private TMP_Text turretName;
    [SerializeField] private TMP_Text turretCost;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = GameObject.Find("GameController");
        buildingSystem = gameController.GetComponent<BuildingSystem>();

        turretName.text = objectToBuild.name;

        if (objectToBuild.GetComponent<Turret>() != null)
        {
            turretCost.text = objectToBuild.GetComponent<Turret>().GetCost().ToString();
            objectToBuild.GetComponent<Turret>().enabled = false;
        }
        else
        {
            turretCost.text = "0";
        }
    }

    public void InitializeObjectViaButton()
    {
        buildingSystem.InitializeObjectThatFollowMouse(objectToBuild);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
