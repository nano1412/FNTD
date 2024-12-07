using UnityEngine;
using static DamageType;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;
    public float damage = 10;            // ��駤�Ҵ�����ͧ����ع�� 10
    public float explosionRadius = 5f; // ����ա�����Դ (��˹����� AOE)
    public bool isExplodeOnStart;
    public DamageType damageType;

    public AudioClip hitSound; // ������§����Ѻ����ع���������
    private AudioSource audioSource; // ��ǤǺ������§


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (isExplodeOnStart)
        {
            HitTarget();
        }

    }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        if(target.CompareTag("Enemy"))
        {
            speed = target.GetComponent<Enemy>().moveSpeed * 20f;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget()
    {
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }
        Explode();
        Destroy(gameObject);
    }

    void Explode()
    {
        // ��������·�����������ա�����Դ
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            Enemy e = collider.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(damage, damageType); // �Ӵ�������Ѻ�ѵ�ٷ������������
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // �ʴ�����ա�����Դ��������͡����ع� Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
