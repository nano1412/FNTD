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
        float movement = 0;

        if (Input.GetKey(KeyCode.A))
        {
            movement -= 1;
            movement *= movespeed * Time.deltaTime;
            player.transform.position += new Vector3(movement, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement = 1;
            movement *= movespeed * Time.deltaTime;
            player.transform.position += new Vector3(movement, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            movement = 1;
            movement *= movespeed * Time.deltaTime;
            player.transform.position += new Vector3(0, 0, movement);
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement = -1;
            movement *= movespeed * Time.deltaTime;
            player.transform.position += new Vector3(0, 0, movement);
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
