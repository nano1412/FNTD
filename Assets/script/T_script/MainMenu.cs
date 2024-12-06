using UnityEngine;
using UnityEngine.SceneManagement; // ����Ѻ�Ѵ��� Scene

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        // ����¹��ѧ Scene �ͧ�� (������ Scene ���س��ͧ���᷹ "GameScene")
        SceneManager.LoadScene("pathfind demo");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit(); // �Դ�ͻ���पѹ
    }
}
