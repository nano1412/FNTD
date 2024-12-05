using UnityEngine;
using static DamageType;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;
    public float damage = 10;            // ตั้งค่าดาเมจของกระสุนเป็น 10
    public float explosionRadius = 5f; // รัศมีการระเบิด (กำหนดระยะ AOE)
    public bool isExplodeOnStart;
    public DamageType damageType;

    public AudioClip hitSound; // ไฟล์เสียงสำหรับกระสุนชนเป้าหมาย
    private AudioSource audioSource; // ตัวควบคุมเสียง


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
        // หาเป้าหมายที่อยู่ในรัศมีการระเบิด
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            Enemy e = collider.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(damage, damageType); // ทำดาเมจให้กับศัตรูที่อยู่ในรัศมี
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // แสดงรัศมีการระเบิดเมื่อเลือกกระสุนใน Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
