using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI; // Canvas �ͧ Pause Menu
    private bool isPaused = false;

    void Update()
    {
        // ������ ESC �����Դ/�Դ Pause Menu
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

    // �ѧ��ѹ�Դ Pause Menu
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // �ʴ� Pause Menu
        Time.timeScale = 0f; // ��ش�������
        isPaused = true;
    }

    // �ѧ��ѹ�Դ Pause Menu
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // ��͹ Pause Menu
        Time.timeScale = 1f; // ���ҡ�Ѻ�һ���
        isPaused = false;
    }

    public void OpenSettings()
    {
        Debug.Log("�Դ Settings!"); // ���ҧ˹�� Settings �����ͧ���
    }

    public void OpenHowToPlay()
    {
        Debug.Log("�Դ How to Play!"); // ���ҧ˹�� How to Play �����ͧ���
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // ���ҡ�Ѻ�һ���
        SceneManager.LoadScene("MainMenu"); // ��Ŵ�ҡ������ѡ
    }
}
