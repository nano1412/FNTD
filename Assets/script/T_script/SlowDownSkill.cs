using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlowDownSkill : MonoBehaviour
{
    public Button slowDownButton; // ���� UI ����Ѻ��ʡ�� Slow Down
    public int skillCost = 150; // �ҤҢͧʡ��
    public float slowDownDuration = 5f; // �������ҷ���ѵ�٨ж١Ŵ��������
    public float slowFactor = 0.5f; // �ѵ�ҡ��Ŵ�������� (50% �ͧ�����������)
    public float cooldownTime = 20f; // ���� Cooldown 20 �Թҷ�

    private bool isCooldown = false; // ��Ǩ�ͺ��ҡ��ѧ����㹪�ǧ Cooldown �������
    private float cooldownTimer; // ��ǨѺ��������Ѻ Cooldown

    void Start()
    {
        if (slowDownButton != null)
        {
            slowDownButton.onClick.AddListener(ActivateSkill);
        }
    }

    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            slowDownButton.interactable = false; // �Դ�����ҹ����㹪�ǧ Cooldown

            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
                slowDownButton.interactable = true; // �Դ�����ҹ��������� Cooldown ����ش
            }
        }
    }

    void ActivateSkill()
    {
        if (isCooldown)
        {
            Debug.Log("Skill is on cooldown.");
            return;
        }

        if (CoinSystem.current.SpendCoins(skillCost))
        {
            Debug.Log("Skill Slow Down activated! Slowing all enemies.");
            ApplySlowDown(); // Ŵ���������ѵ�ٷء���
            StartCooldown(); // ����� Cooldown
        }
        else
        {
            Debug.Log("Not enough coins to use Slow Down.");
        }
    }

    private void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = cooldownTime;
        Debug.Log($"Slow Down skill on cooldown for {cooldownTime} seconds.");
    }

    void ApplySlowDown()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>(); // �� Enemy �ء���㹩ҡ

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.moveSpeed *= slowFactor; // Ŵ��������ŧ��� slowFactor
                Debug.Log($"{enemy.name} speed reduced.");
            }
        }

        StartCoroutine(ResetEnemySpeedAfterDuration());
    }

    private IEnumerator ResetEnemySpeedAfterDuration()
    {
        yield return new WaitForSeconds(slowDownDuration);

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.moveSpeed /= slowFactor; // �׹��Ҥ������ǻѨ�غѹ
                Debug.Log($"{enemy.name} speed restored.");
            }
        }
    }
}
