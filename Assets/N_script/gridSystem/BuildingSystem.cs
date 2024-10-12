using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;

    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    private GameObject objectToPlace;
    private PlaceableObject objectToPlace_PlaceableObjectScript;

    public int turretCost = 200; // กำหนดค่าใช้จ่ายในการวาง turret

    #region Unity methods

    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Destroy(objectToPlace);
            InitializeWithObject(prefab1);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Destroy(objectToPlace);
            InitializeWithObject(prefab2);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Destroy(objectToPlace);
            InitializeWithObject(prefab3);
        }

        if (!objectToPlace_PlaceableObjectScript)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            objectToPlace_PlaceableObjectScript.Rotate();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!CanBePlaced(objectToPlace_PlaceableObjectScript))
            {
                Debug.Log("invalid placement.");
                Destroy(objectToPlace_PlaceableObjectScript.gameObject);
            }

            if (!CoinSystem.SpendCoins(turretCost))
            {
                Debug.Log("Not enough coins to place the turret.");
                Destroy(objectToPlace_PlaceableObjectScript.gameObject);
            }

            objectToPlace_PlaceableObjectScript.Place();
            Vector3Int start = gridLayout.WorldToCell(objectToPlace_PlaceableObjectScript.GetStartPosition());
            TakeArea(start, objectToPlace_PlaceableObjectScript.Size);
             
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(objectToPlace_PlaceableObjectScript.gameObject);
        }
    }

    #endregion

    #region Utils

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    #endregion

    #region Building Placement

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        objectToPlace = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace_PlaceableObjectScript = objectToPlace.GetComponent<PlaceableObject>();
        objectToPlace.AddComponent<ObjectDrag>();
    }

    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace_PlaceableObjectScript.GetStartPosition());
        area.size = placeableObject.Size;
        area.size = new Vector3Int(area.size.x + 1, area.size.y + 1, area.size.z);

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, whiteTile, start.x, start.y,
                        start.x + size.x, start.y + size.y);
    }

    #endregion
}
