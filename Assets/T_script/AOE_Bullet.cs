using UnityEngine;

public class AOE_Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;
    public int damage = 10; // ดาเมจที่ศัตรูทุกตัวจะได้รับ
    public float explosionRadius = 5f; // รัศมีการระเบิด

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // ถ้าเป้าหมายหายไป ทำลายกระสุน
            return;
        }

        Vector3 dir = target.position - transform.position; // คำนวณทิศทางไปหาเป้าหมาย
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            Explode(); // ถ้ากระสุนอยู่ใกล้เป้าหมายพอ ให้ทำการระเบิด
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World); // เคลื่อนที่กระสุน
        transform.LookAt(target); // ให้กระสุนหมุนไปตามเป้าหมาย
    }

    void Explode()
    {
        // สร้างการระเบิดเป็นวงกลมที่มีระยะเท่ากับ explosionRadius
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, explosionRadius);

        // ทำดาเมจให้กับศัตรูทุกตัวที่อยู่ในรัศมีการระเบิด
        foreach (Collider enemy in hitEnemies)
        {
            Enemy e = enemy.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(damage); // ทำดาเมจให้กับศัตรู
            }
        }

        // ทำลายกระสุนหลังจากการระเบิด
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // แสดงรัศมีการระเบิดใน Scene View เพื่อช่วยในการดีบัก
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
