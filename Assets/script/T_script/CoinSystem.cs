using UnityEngine;
using TMPro;

public class CoinSystem : MonoBehaviour
{
    public static CoinSystem current;

    public int Coins; // �ӹǹ����­�Ѩ�غѹ
    public int startCoins = 100; // �ӹǹ����­�������

    public TMP_Text coinText; // ����ʴ�����­� UI

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        Coins = startCoins; // ��˹��ӹǹ����­�������
        //Debug.Log("Game started with " + Coins + " coins."); // �ʴ���� Console
        UpdateCoinText(); // �Ѿഷ UI �ͧ����­
    }

    // �ѧ��ѹ����Ѻ����ѡ����­
    public bool SpendCoins(int amount)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
            //Debug.Log("Coins spent: " + amount + ". Current coins: " + Coins);
            return true;
        }
        else
        {
            //Debug.Log("Not enough coins. Current coins: " + Coins);
            return false;
        }
    }

    // �ѧ��ѹ����Ѻ�����������­
    public void AddCoins(int amount)
    {
        Coins += (int) (amount * WaveController.current.CoinRewardbase);
        //Debug.Log("Coins added: " + amount + ". Current coins: " + Coins);
    }

    // �Ѿഷ UI �ͧ����­
    private void Update()
    {
        UpdateCoinText();
    }

    // �ʴ��ӹǹ����­��˹�Ҩ�
    private void UpdateCoinText()
    {
        coinText.text = Coins.ToString();
    }
}