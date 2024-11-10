using UnityEngine;
using UnityEngine.Playables;

public class path : MonoBehaviour
{
    public GameObject ToPath;
    public float moveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
            if(ToPath == null)
        {
            ToPath = BuildingSystem.current.humanKingdom;
        }
            transform.position = Vector3.MoveTowards(transform.position, ToPath.transform.position, moveSpeed * Time.deltaTime);

            //how enemy know where to move next
            if(Vector3.Distance(transform.position, ToPath.transform.position) <= 5 && ToPath != BuildingSystem.current.humanKingdom)
            {
            ToPath = ToPath.GetComponent<path_linkedlist>().nextPath;
            transform.parent = ToPath.transform;
            }
        
    }
}
