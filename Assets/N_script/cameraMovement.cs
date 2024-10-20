using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class cameraMovement : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;

    [SerializeField] CinemachineCamera virtualCamera;
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
            transform.position += transform.right * -1 * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += transform.forward * -1 * moveSpeed * Time.deltaTime;
        }

        float camX = Mathf.Clamp(transform.position.x, -BuildingSystem.current.buildingRange, BuildingSystem.current.buildingRange);
        float camY = transform.position.y;
        float camZ = Mathf.Clamp(transform.position.z, -BuildingSystem.current.buildingRange, BuildingSystem.current.buildingRange);

        transform.position = new Vector3(camX,camY,camZ);

        //transform.position = Vector3.ClampMagnitude(cameraFocus.transform.position, BuildingSystem.current.buildingRange);

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, rotateSpeed, 0);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, -rotateSpeed, 0);
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
