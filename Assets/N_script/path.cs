using UnityEngine;
using UnityEngine.Playables;

public class path : MonoBehaviour
{
    public Transform[] points;
    public float moveSpeed;

    [SerializeField]  private int pointsIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = points[pointsIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(pointsIndex < points.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[pointsIndex].transform.position, moveSpeed * Time.deltaTime);

            if(transform.position == points[pointsIndex].transform.position)
            {
                pointsIndex += 1;
            }
        }
    }

    public void AddPath(Transform[] points)
    {
        this.points = points;
    }
}
