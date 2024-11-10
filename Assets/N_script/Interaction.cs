using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEditor;

public class Interaction : MonoBehaviour
{
    public static Interaction current;
    Ray ray;
    RaycastHit hit;
    [SerializeField] private GameObject createNewPathCanvas;
    [SerializeField] private GameObject EnemyStatCanvas;
    [SerializeField] private GameObject TowerStatCanvas;
    [SerializeField] private GameObject DisableCanvasButton;
    [SerializeField] private Button upgradeTowerButton; // Reference to the Upgrade button

    public GameObject saveHit;
    [SerializeField] private GameObject selected;
    [SerializeField] private GameObject selectedCanvas;
    [SerializeField] private create_new_path create_new_path_script;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        DisableAllCanvas();
        upgradeTowerButton.onClick.AddListener(OnUpgradeTowerButtonClicked);
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {

                
                saveHit = hit.collider.gameObject;

        }

        /*
        if (saveHit.tag == "Spawner")
        {
            ActiveSpawnerCanvas();
            saveSpawner = saveHit.transform.Find("spawner").gameObject;
            saveHit = null;
        }*/
        if (Input.GetMouseButtonDown(0))
        {
            print(hit.collider.tag);
            try
            {
                switch (saveHit.tag)
                {
                    case "Spawner":
                        ActiveCanvas(createNewPathCanvas);
                        selectedCanvas = createNewPathCanvas;
                        selected = saveHit.gameObject;
                        break;

                    case "Enemy":
                        ActiveCanvas(EnemyStatCanvas);
                        selectedCanvas = EnemyStatCanvas;
                        selected = saveHit.gameObject;
                        SetEmenyStatInCanvas(selected);
                        break;

                    case "Tower":
                        ActiveCanvas(TowerStatCanvas);
                        selectedCanvas = TowerStatCanvas;
                        selected = saveHit.gameObject;
                        SetTowerStatInCanvas(selected);
                        break;

                    //not implement yet
                    case "HumanKingdom":
                        ActiveCanvas(null);
                        selectedCanvas = null;
                        selected = saveHit.transform.Find("HumanKingdom").gameObject;
                        break;


                    case null:

                        break;

                    default:

                        break;
                }
            }

            catch (MissingReferenceException ex)
            {
                //Debug.Log("saveHit is null");
            }

            catch (UnassignedReferenceException ex)
            {
                //Debug.Log("the game is just run, there is nothing in saveHit yet");
            }
        }
    }
    

    void SetEmenyStatInCanvas(GameObject enemy)
    {
        if(enemy.tag != "Enemy")
        {
            Debug.Log("This is not Enemy");
            return;
        }
        TMP_Text nameCanvas = EnemyStatCanvas.transform.Find("EnemyName").GetComponent<TMP_Text>();
        TMP_Text speedCanvas = EnemyStatCanvas.transform.Find("EnemySpeed").GetComponent<TMP_Text>();
        TMP_Text hpCanvas = EnemyStatCanvas.transform.Find("EnemyHP").GetComponent<TMP_Text>();

        string enemyName = enemy.name;
        string enemySpeed = enemy.GetComponent<path>().moveSpeed.ToString();
        string enemyHP = enemy.GetComponent<Enemy>().maxHP.ToString();

        nameCanvas.text = enemyName;
        speedCanvas.text = "Speed: " + enemySpeed;
        hpCanvas.text = "max HP: " + enemyHP;
    }

    void SetTowerStatInCanvas(GameObject tower)
    {
        if (tower.tag != "Tower")
        {
            Debug.Log("This is not Tower");
            return;
        }

        TMP_Text nameCanvas = TowerStatCanvas.transform.Find("TowerName").GetComponent<TMP_Text>();
        TMP_Text lvCanvas = TowerStatCanvas.transform.Find("TowerLevel").GetComponent<TMP_Text>();
        TMP_Text bulletCanvas = TowerStatCanvas.transform.Find("TowerBulletType").GetComponent<TMP_Text>();
        TMP_Text rangeCanvas = TowerStatCanvas.transform.Find("TowerRange").GetComponent<TMP_Text>();
        TMP_Text fireRateCanvas = TowerStatCanvas.transform.Find("TowerfireRate").GetComponent<TMP_Text>();
        TMP_Text damageCanvas = TowerStatCanvas.transform.Find("TowerDamage").GetComponent<TMP_Text>();

        string towerName = tower.name;
        string towerlv = "0"; //not implement yet
        string towerBullet = tower.GetComponent<Turret>().bulletPrefab.name;
        string towerRange = tower.GetComponent<Turret>().range.ToString();
        string towerFireRate = tower.GetComponent<Turret>().fireRate.ToString();
        string towerDamage = tower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().damage.ToString();

        nameCanvas.text = towerName;
        lvCanvas.text = "LV: " + towerlv;
        bulletCanvas.text = "Bullet: " + towerBullet;
        rangeCanvas.text = "Range: " + towerRange;
        fireRateCanvas.text = "FireRate: " + towerFireRate;
        damageCanvas.text = "Damage: " + towerDamage;


    }

    private void OnUpgradeTowerButtonClicked()
    {
        if (selected != null && selected.CompareTag("Tower"))
        {
            Turret turret = selected.GetComponent<Turret>();
            if (turret != null)
            {
                int upgradeCost = turret.upgradeCost; // ค่าใช้จ่ายในการอัปเกรด
                if (CoinSystem.current.SpendCoins(upgradeCost)) // ตรวจสอบและหักเหรียญสำหรับการอัปเกรด
                {
                    turret.UpgradeTurret(); // อัปเกรดป้อม
                    selected = null; // รีเซ็ตการเลือกป้อมให้กลับไปอยู่สถานะเริ่มต้น
                    DisableAllCanvas(); // ปิดหรือรีเฟรช UI ให้กลับไปอยู่สถานะเริ่มต้น
                }
                else
                {
                    Debug.Log("เหรียญไม่พอสำหรับการอัปเกรด!");
                }
            }
        }
    }

    void ActiveCanvas(GameObject canvas)
    {
        Debug.Log("active canvas called");
        DisableAllCanvas();

        canvas.SetActive(true);
        DisableCanvasButton.SetActive(true);
    }

    public void DisableAllCanvas()
    {
        Debug.Log("disable canvas called");
        createNewPathCanvas.SetActive(false);
        EnemyStatCanvas.SetActive(false);
        TowerStatCanvas.SetActive(false);
        DisableCanvasButton.SetActive(false);
    }

    public void ChangeSpawnerPath()
    {
        create_new_path_script.ChangePath(selected);
    }
}
