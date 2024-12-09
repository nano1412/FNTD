using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEditor;

public class Interaction : MonoBehaviour
{
    public static Interaction current;
    public GameObject curser3DPrefab;
    public GameObject curser3D;
    private Vector3 offset;

    Ray ray;
    public RaycastHit hit;
    [SerializeField] private GameObject createNewPathCanvas;
    [SerializeField] private GameObject EnemyStatCanvas;
    [SerializeField] private GameObject TowerStatCanvas;
    [SerializeField] private GameObject DisableCanvasButton;
    [SerializeField] private Button upgradeTowerButton; // Reference to the Upgrade button
    [SerializeField] private GameObject gameOverPanel; // Panel ����Ѻ˹�� Game Over
    [SerializeField] private Button retryButton; // ���� Retry
    [SerializeField] private Button mainMenuButton; // ���� Main Menu

    GameObject saveTurretRangeIndicator;
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

        curser3D = Instantiate(curser3DPrefab, transform);
        transform.GetComponent<create_new_path>().curser3D = curser3D;

        curser3D.SetActive(false);

        retryButton.onClick.AddListener(RetryGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);

        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (PlayerStats.current.Lives <= 0)
        {
            GameOver(); // ���¡�ѧ��ѹ Game Over
        }

        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + offset;
        curser3D.transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            saveHit = hit.collider.gameObject;
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (!saveHit) return;
            switch (saveHit.tag)
            {
                case "Spawner":
                    //change method to called create new path to create_new_path.cs NewChangePath() still keep this in case we have interaction menu with spawner

                    //ActiveCanvas(createNewPathCanvas);
                    //selectedCanvas = createNewPathCanvas;
                    //selected = saveHit.gameObject;
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
                    //ActiveCanvas(null);
                    //selectedCanvas = null;
                    //selected = saveHit.transform.Find("HumanKingdom").gameObject;
                    break;

                default:

                    break;
            }

        }

    }

    public void GameOver()
    {
        // �ʴ�˹�� Game Over Panel
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // ��ش�������
    }

    public void RetryGame()
    {
        Time.timeScale = 1f; // �����������

        ScoreController.current.ResetScore();
        WaveController.current.ResetWave(); // ���� wave
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        // ��Ŵ Scene ������ѡ
        Time.timeScale = 1f; // �����������
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }


    void SetEmenyStatInCanvas(GameObject enemy)
    {
        if (enemy.tag != "Enemy")
        {
            Debug.Log("This is not Enemy");
            return;
        }
        TMP_Text nameCanvas = EnemyStatCanvas.transform.Find("EnemyName").GetComponent<TMP_Text>();
        TMP_Text speedCanvas = EnemyStatCanvas.transform.Find("EnemySpeed").GetComponent<TMP_Text>();
        TMP_Text hpCanvas = EnemyStatCanvas.transform.Find("EnemyHP").GetComponent<TMP_Text>();
        TMP_Text physicalResistanceCanvas = EnemyStatCanvas.transform.Find("EnemyPhysicalResist").GetComponent<TMP_Text>();
        TMP_Text magicResistanceCanvas = EnemyStatCanvas.transform.Find("EnemyMagicResist").GetComponent<TMP_Text>();

        string enemyName = enemy.GetComponent<Enemy>().enemyName;
        string enemySpeed = enemy.GetComponent<path>().moveSpeed.ToString();
        string enemyHP = enemy.GetComponent<Enemy>().maxHP.ToString();
        string enemyPhysicalResistance = enemy.GetComponent<Enemy>().physicalResistance.ToString();
        string enemyMagicResistance = enemy.GetComponent<Enemy>().magicResistance.ToString();

        nameCanvas.text = enemyName;
        speedCanvas.text = "Speed: " + enemySpeed;
        hpCanvas.text = "max HP: " + enemyHP;
        physicalResistanceCanvas.text = "Physical Resistance: " + enemyPhysicalResistance + " %";
        magicResistanceCanvas.text = "Physical Resistance: " + enemyMagicResistance + " %";
    }

    void SetTowerStatInCanvas(GameObject tower)
    {
        if (tower.tag != "Tower")
        {
            Debug.Log("This is not Tower");
            return;
        }

        //show indicator
        saveTurretRangeIndicator = tower.GetComponent<Turret>().rangeIndicator;
        saveTurretRangeIndicator.SetActive(true);

        TMP_Text upgradeCostCanvas = TowerStatCanvas.transform.Find("TowerUpgradeCost").GetComponent<TMP_Text>();
        TMP_Text nameCanvas = TowerStatCanvas.transform.Find("TowerName").GetComponent<TMP_Text>();
        TMP_Text lvCanvas = TowerStatCanvas.transform.Find("TowerLevel").GetComponent<TMP_Text>();
        TMP_Text bulletCanvas = TowerStatCanvas.transform.Find("TowerBulletType").GetComponent<TMP_Text>();
        TMP_Text rangeCanvas = TowerStatCanvas.transform.Find("TowerRange").GetComponent<TMP_Text>();
        TMP_Text fireRateCanvas = TowerStatCanvas.transform.Find("TowerfireRate").GetComponent<TMP_Text>();
        TMP_Text damageCanvas = TowerStatCanvas.transform.Find("TowerDamage").GetComponent<TMP_Text>();

        string upgradeCost = tower.GetComponent<Turret>().upgradeCost.ToString();
        string towerName = tower.GetComponent<Turret>().turretName;
        string towerlv = tower.GetComponent<Turret>().level.ToString(); //not implement yet
        string towerBullet = tower.GetComponent<Turret>().bulletPrefab.name;
        string towerRange = tower.GetComponent<Turret>().range.ToString();
        string towerFireRate = tower.GetComponent<Turret>().fireRate.ToString();
        string towerDamage = tower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().damage.ToString();

        upgradeCostCanvas.text = "Upgradde cost:" + upgradeCost + " C";
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
                int upgradeCost = turret.upgradeCost;
                if (CoinSystem.current.SpendCoins(upgradeCost))
                {
                    turret.UpgradeTurret();
                    // อัปเดต selected ให้ชี้ไปยัง Turret ตัวใหม่
                    selected = turret.gameObject;
                    DisableAllCanvas();
                }
            }
        }
    }


    void ActiveCanvas(GameObject canvas)
    {
        //Debug.Log("active canvas called");
        DisableAllCanvas();

        canvas.SetActive(true);
        DisableCanvasButton.SetActive(true);
    }

    public void DisableAllCanvas()
    {
        //Debug.Log("disable canvas called");
        createNewPathCanvas.SetActive(false);
        EnemyStatCanvas.SetActive(false);
        TowerStatCanvas.SetActive(false);
        DisableCanvasButton.SetActive(false);

        if (saveTurretRangeIndicator != null)
        {
            saveTurretRangeIndicator.SetActive(false);
        }
    }

    public void ChangeSpawnerPath()
    {
        create_new_path_script.ChangePath(selected);
    }
}
