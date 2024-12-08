using UnityEngine;
using TMPro;

public class Building_Button : MonoBehaviour
{
    [SerializeField] private GameObject gameController;
    [SerializeField] private int buttonIndex;
    [SerializeField] private GameObject objectToBuild;

    [SerializeField] private TMP_Text turretName;
    [SerializeField] private TMP_Text turretCost;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = GameObject.Find("GameController");

        turretName.text = objectToBuild.GetComponent<Turret>().turretName;

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
        BuildingSystem.current.InitializeObjectThatFollowMouse(objectToBuild);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
