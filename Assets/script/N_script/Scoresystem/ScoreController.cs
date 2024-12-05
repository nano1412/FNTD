using System;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static ScoreController current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
