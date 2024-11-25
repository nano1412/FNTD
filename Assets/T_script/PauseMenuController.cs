using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI; // Canvas ของ Pause Menu
    private bool isPaused = false;

    void Update()
    {
        // กดปุ่ม ESC เพื่อเปิด/ปิด Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // ฟังก์ชันเปิด Pause Menu
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // แสดง Pause Menu
        Time.timeScale = 0f; // หยุดเวลาในเกม
        isPaused = true;
    }

    // ฟังก์ชันปิด Pause Menu
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // ซ่อน Pause Menu
        Time.timeScale = 1f; // เวลากลับมาปกติ
        isPaused = false;
    }

    public void OpenSettings()
    {
        Debug.Log("เปิด Settings!"); // สร้างหน้า Settings ตามต้องการ
    }

    public void OpenHowToPlay()
    {
        Debug.Log("เปิด How to Play!"); // สร้างหน้า How to Play ตามต้องการ
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // เวลากลับมาปกติ
        SceneManager.LoadScene("MainMenu"); // โหลดฉากเมนูหลัก
    }
}
