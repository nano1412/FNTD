using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public static ScoreController current;

    public int currentTotalKill;
    public int currentCommonKill;
    public int currentRareKill;
    public int currentBossKill;
    public string currentPlayer;

    public GameObject totalKillText;
    public GameObject commonKillText;
    public GameObject rareKillText;
    public GameObject bossKillText;
    public GameObject playerNameInput;

    private bool isAlreadyAddScore = false;

    List<ScoreElement> scores = new List<ScoreElement>();
    [SerializeField] int maxShowScoreCount = 5;
    [SerializeField] string saveFileName;

    private void Awake()
    {
        current = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadScore();
    }
    
    private void LoadScore()
    {
        scores = FileHandler.ReadListFromJSON<ScoreElement>(saveFileName);

        //data lower than in leaderboard will be delete
        while (scores.Count > maxShowScoreCount) {
            scores.RemoveAt(maxShowScoreCount);
        }
    }

    private void SaveScore()
    {
        FileHandler.SaveToJSON<ScoreElement>(scores, saveFileName);
    }

    public void AddHighscoreIfPossible(ScoreElement element)
    {
        if (isAlreadyAddScore) { return; }

        for(int i = 0;i < maxShowScoreCount; i++)
        {
            if(i >= scores.Count || element.totalKill > scores[i].totalKill)
            {
                //add new highscore
                scores.Insert(i, element);
                isAlreadyAddScore = true;

                while (scores.Count > maxShowScoreCount)
                {
                    scores.RemoveAt(maxShowScoreCount);
                }

                SaveScore();
                break;
            }
        }
    }

    public void ResetScore()
    {
    currentTotalKill = 0;
    currentCommonKill = 0;
    currentRareKill = 0;
    currentBossKill = 0;
    currentPlayer = "";
        isAlreadyAddScore= false;
    }

    public void AddCommonKill()
    {
        currentCommonKill++;
    }

    public void AddRareKill()
    {
        currentRareKill++;
    }

    public void AddBossKill()
    {
        currentBossKill++;
    }

    public void AddPlayername(string name)
    {
        currentPlayer = name;
    }

    public void SummiitToLeaderBoard()
    {
        AddPlayername(playerNameInput.GetComponent<TMP_InputField>().text);

        ScoreElement currentPlayerScore = new ScoreElement
        (
            currentPlayer,
            currentTotalKill,
            currentCommonKill,
            currentRareKill,
            currentBossKill
        );

        AddHighscoreIfPossible(currentPlayerScore);
    }

    private void Update()
    {
        currentTotalKill = currentCommonKill + currentRareKill +currentBossKill;

        totalKillText.GetComponent<TMP_Text>().text = currentTotalKill.ToString();
        commonKillText.GetComponent<TMP_Text>().text=currentCommonKill.ToString();
        rareKillText.GetComponent<TMP_Text>().text = currentRareKill.ToString();
        bossKillText.GetComponent<TMP_Text>().text =currentBossKill.ToString();
    }
}

[Serializable]
public class ScoreElement
{
    public string player;
    public int totalKill;
    public int commonKill;
    public int rareKill;
    public int bossKill;

    public ScoreElement(string player, int totalKill, int commonKill, int rareKill, int bossKill)
    {
        this.player = player;
        this.totalKill = totalKill;
        this.commonKill = commonKill;
        this.rareKill = rareKill;
        this.bossKill = bossKill;
    }
}
