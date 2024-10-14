using Unity.Cinemachine;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class cameraMovement : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    public GameObject player;
    float smooth = 5.0f;

    [SerializeField] CinemachineCamera virtualCamera;
    [SerializeField] BuildingSystem buildingSystem;
    CinemachineComponentBase componentBase;
    float cameraDistance;
    [SerializeField] float sensitivity = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementAndRotation();
        Zoom();
    }

    void MovementAndRotation()
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

        player.transform.position = Vector3.ClampMagnitude(player.transform.position, buildingSystem.buildingRange);

        if (Input.GetKey(KeyCode.Q))
        {
            player.transform.Rotate(0, rotateSpeed, 0);
        }

        if (Input.GetKey(KeyCode.E))
        {
            player.transform.Rotate(0, -rotateSpeed, 0);
        }
    }

    void Zoom()
    {
        if(componentBase == null)
        {
            componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }

        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraDistance = Input.GetAxis("Mouse ScrollWheel") * sensitivity;
            if(componentBase is CinemachinePositionComposer)
            {
                (componentBase as CinemachinePositionComposer).CameraDistance -= cameraDistance;
            }
        }
    }
}
