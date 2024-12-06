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

    [SerializeField] private GameObject totalKillText;
    [SerializeField] private GameObject commonKillText;
    [SerializeField] private GameObject rareKillText;
    [SerializeField] private GameObject bossKillText;
    [SerializeField] private GameObject playerNameInput;

    private bool isAlreadyAddScore = false;

    [SerializeField] private Transform scoreParent;
    [SerializeField] private GameObject scorePrefab;
    List<GameObject> scoresUI = new List<GameObject>();

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

    private void UpdateUI(List<ScoreElement> elements)
    {
        for(int i = 0; i < elements.Count; i++)
        {
            ScoreElement element = elements[i];
            if(element.totalKill > 0) {

                if (i >= scoresUI.Count)
                {
                    GameObject temp = Instantiate(scorePrefab,Vector3.zero,Quaternion.identity);
                    temp.transform.SetParent(scoreParent,false);

                    scoresUI.Add(temp);

                }

                var texts = scoresUI[i].GetComponentsInChildren<TMP_Text>();
                texts[0].text = element.player;
                texts[1].text = element.totalKill.ToString();
                texts[2].text = element.commonKill.ToString();
                texts[3].text = element.rareKill.ToString();
                texts[4].text = element.bossKill.ToString();

            }
        }
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
