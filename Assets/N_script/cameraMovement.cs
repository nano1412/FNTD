using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class cameraMovement : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    public GameObject player;
    float smooth = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.A))
        {
            player.transform.position += transform.right * -1 * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.position += transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            player.transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.transform.position += transform.forward * -1 * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            player.transform.Rotate(0, rotateSpeed, 0);
        }

        if (Input.GetKey(KeyCode.E))
        {
            player.transform.Rotate(0, -rotateSpeed, 0);
        }
    }
}
