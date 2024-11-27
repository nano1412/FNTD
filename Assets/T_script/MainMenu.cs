using UnityEngine;
using UnityEngine.SceneManagement; // สำหรับจัดการ Scene

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        // เปลี่ยนไปยัง Scene ของเกม (ใส่ชื่อ Scene ที่คุณต้องการแทน "GameScene")
        SceneManager.LoadScene("pathfind demo");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit(); // ปิดแอปพลิเคชัน
    }
}
