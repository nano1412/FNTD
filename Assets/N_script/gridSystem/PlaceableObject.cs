using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool Placed;
    public Vector3Int Size { get; private set; }
    [SerializeField] private Vector3[] Vertices;

    public bool GetColliderVertexPositionsLocal()
    {
        BoxCollider box_collider = gameObject.GetComponent<BoxCollider>();
        Vertices = new Vector3[4];
        Vertices[0] = box_collider.center + new Vector3(-box_collider.size.x, -box_collider.size.y, -box_collider.size.z) * 0.5f;
        Vertices[1] = box_collider.center + new Vector3(box_collider.size.x, -box_collider.size.y, -box_collider.size.z) * 0.5f;
        Vertices[2] = box_collider.center + new Vector3(box_collider.size.x, -box_collider.size.y, box_collider.size.z) * 0.5f;
        Vertices[3] = box_collider.center + new Vector3(-box_collider.size.x, -box_collider.size.y, box_collider.size.z) * 0.5f;

        return true;
    }

    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[Vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(Vertices[i]);
            vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }

        Size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x),
                                Math.Abs((vertices[0] - vertices[3]).y),
                                1);
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }

    private void Start()
    {
        GetColliderVertexPositionsLocal();
        CalculateSizeInCells();
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));
        Size = new Vector3Int(Size.y, Size.x, 1);

        Vector3[] vertices = new Vector3[Vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Vertices[(i + 1) % Vertices.Length];
        }

        Vertices = vertices;
    }

    public virtual void Place()
    {


        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);

        Placed = true;

        //invoke events of placement
    }
}