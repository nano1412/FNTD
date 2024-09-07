using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static bool GameIsOver;

    public GameObject gameOverUI;
    public GameObject completeLevelUI;

    void Start()
    {
        GameIsOver = false;
        Debug.Log("Game started. GameIsOver set to " + GameIsOver);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameIsOver)
            return;

        if (PlayerStats.Lives <= 0)
        {
            Debug.Log("Player has no lives left. Ending game...");
            EndGame();
        }
    }

    void EndGame()
    {
        GameIsOver = true;
        Debug.Log("Game over. GameIsOver set to " + GameIsOver);
    }
}