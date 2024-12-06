using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DynamiteSkill : MonoBehaviour
{
    public DamageType damageType;
    public int skillCost = 100; // �ҤҢͧʡ��
    public float skillDamage = 50; // ����������·��ʡ�ŷ�
    public float cooldownTime = 30f; // ���� Cooldown 30 �Թҷ�
    public float explosionRadius = 5f; // ����բͧ������� AOE

    private bool isCooldown = false; // ��Ǩ�ͺ��ҡ��ѧ����㹪�ǧ Cooldown �������
    [SerializeField] private float cooldownTimer; // ��ǨѺ��������Ѻ Cooldown

    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;


            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
            }
        }
    }

    public void ActivateSkill()
    {
        if (isCooldown)
        {
            Debug.Log("Skill is on cooldown.");
            return;
        }

        if (CoinSystem.current.SpendCoins(skillCost))
        {
            Debug.Log("Skill Dynamite activated! Select a location.");
            StartCoroutine(SelectLocationAndDamage());
            StartCooldown(); // ����� Cooldown
        }
        else
        {
            Debug.Log("Not enough coins to use Dynamite.");
        }
    }

    private void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = cooldownTime;
        Debug.Log($"Dynamite skill on cooldown for {cooldownTime} seconds.");
    }

    private IEnumerator SelectLocationAndDamage()
    {
        bool locationSelected = false;

        while (!locationSelected)
        {
            if (Input.GetMouseButtonDown(0)) // ����ͤ�ԡ����
            {

                Vector3 explosionPoint = Interaction.current.hit.point; // ���˹觷�����Դ
                Collider[] colliders = Physics.OverlapSphere(explosionPoint, explosionRadius); // ��Ǩ�ͺ�ѵ��������

                foreach (Collider nearbyObject in colliders)
                {
                    Enemy enemy = nearbyObject.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(skillDamage, damageType);
                        Debug.Log($"{enemy.name} took {skillDamage} damage from Dynamite.");
                    }
                }

                Debug.Log($"Dynamite exploded at {explosionPoint} with radius {explosionRadius}.");
                locationSelected = true;

            }

            if (Input.GetMouseButtonDown(1)) // ����ͤ�ԡ���
            {
                Debug.Log("Skill Dynamite canceled.");
                CoinSystem.current.AddCoins(skillCost); // �׹�������­�����¡��ԡ
                yield break; // ¡��ԡ Coroutine
            }

            yield return null; // ������Ѵ�
        }
    }
}
