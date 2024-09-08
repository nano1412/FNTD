using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class cameraMovement : MonoBehaviour
{
    public float movespeed = 1000;
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
            player.transform.position += transform.right * -1 * movespeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.position += transform.right * movespeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            player.transform.position += transform.forward * movespeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.transform.position += transform.forward * -1 * movespeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            player.transform.Rotate(0, 5, 0);
        }

        if (Input.GetKey(KeyCode.E))
        {
            player.transform.Rotate(0, -5, 0);
        }
    }
}
