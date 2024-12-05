using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static ScoreController current;

    List<ScoreElement> scores = new List<ScoreElement>();
    [SerializeField] int maxShowScoreCount = 5;
    [SerializeField] string saveFileName;
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
        for(int i = 0;i < maxShowScoreCount; i++)
        {
            if(i >= scores.Count || element.totalKill > scores[i].totalKill)
            {
                //add new highscore
                scores.Insert(i, element);

                while (scores.Count > maxShowScoreCount)
                {
                    scores.RemoveAt(maxShowScoreCount);
                }

                SaveScore();
                break;
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
