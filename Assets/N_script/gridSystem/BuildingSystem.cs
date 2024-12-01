using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BuildingSystem : MonoBehaviour
{
    public float timeScale = 1f;
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;
    public GameObject humanKingdom;
    public GameObject paths;
    [SerializeField] private GameObject floor;

    public float buildingRange;
    public float noRNGSpawnRange;

    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    private GameObject objectToPlace;
    private PlaceableObject objectToPlace_PlaceableObjectScript;

    private GameObject objectToPlaceRNG;
    private PlaceableObject objectToPlaceRNG_PlaceableObjectScript;

    private int objectToPlaceCost;
    #region Unity methods

    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        Time.timeScale = timeScale;

        //noRNGSpawnRange formula
        noRNGSpawnRange = buildingRange / 3;
        
        KeyboardInput();

        floor.transform.localScale = new Vector3(buildingRange * 2, buildingRange * 2, buildingRange * 2);
    }

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            InitializeObjectThatFollowMouse(prefab1);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            InitializeObjectThatFollowMouse(prefab2);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            InitializeObjectThatFollowMouse(prefab3);
        }

        if (!objectToPlace_PlaceableObjectScript)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            objectToPlace_PlaceableObjectScript.Rotate();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && objectToPlace != null)
        {
            if (!IsColideWithWhiteTile(objectToPlace_PlaceableObjectScript))
            {
                Debug.Log("invalid placement.");
                Destroy(objectToPlace);
            }

            else if (!CoinSystem.current.SpendCoins(objectToPlaceCost))
            {
                Debug.Log("Not enough coins to place the turret.");
                Destroy(objectToPlace);
            }

            else if (!IsInRange())
            {
                Debug.Log("Turret is place to far from the Kingdom");
                Destroy(objectToPlace);
            }

            else
            {
                objectToPlace_PlaceableObjectScript.Place();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace_PlaceableObjectScript.GetStartPosition());
                TakeArea(start, objectToPlace_PlaceableObjectScript.Size);
                if (objectToPlace.GetComponent<Turret>() != null) objectToPlace.GetComponent<Turret>().enabled = true;
            }

            objectToPlace = null;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(objectToPlace);
        }
    }

    public void RNGBuilding(int ObjectAmount, GameObject[] gameObjectPrefab)
    {
        int attempt = 0;
        //spawner
        while (ObjectAmount > 0)
        {
            Vector3 randomPosition = new Vector3(Random.Range(0, BuildingSystem.current.buildingRange) * RandomSign(), 0.6f, Random.Range(0, BuildingSystem.current.buildingRange) * RandomSign());
            Vector3 randomPositionSnap = BuildingSystem.current.SnapCoordinateToGrid(randomPosition);

            bool isInValidSpace = (IsInSquare(BuildingSystem.current.buildingRange, randomPositionSnap.x, randomPositionSnap.z)
                               && !IsInSquare(BuildingSystem.current.noRNGSpawnRange, randomPositionSnap.x, randomPositionSnap.z));


            if (isInValidSpace)
            {
                try
                {
                    if (BuildingSystem.current.InitializeObjectRNG(gameObjectPrefab[Random.Range(0, gameObjectPrefab.Length)], randomPositionSnap))
                    {
                        ObjectAmount--;
                    }
                    else
                    {
                        attempt++;
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    //Debug.LogError(ex.Message);
                    attempt++;
                }
            }

            if (attempt > 20)
            {
                Debug.LogError("attempt to spawn Enemies spawner are excess 20, stop the function");
                break;
            }
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

    private static int RandomSign()
    {
        return Random.value < 0.5f ? 1 : -1;
    }

    private bool IsInSquare(float range, float x, float y)
    {
        bool isXInRange = (x <= range && x >= -range);
        bool isYInRange = (y <= range && y >= -range);

        return isXInRange && isYInRange;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(buildingRange * 2, 100, buildingRange * 2));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(noRNGSpawnRange * 2, 100, noRNGSpawnRange * 2));
    }

    #endregion

    #region Building Placement

    public void InitializeObjectThatFollowMouse(GameObject prefab)
    {
        Destroy(objectToPlace);
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        objectToPlace = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace_PlaceableObjectScript = objectToPlace.GetComponent<PlaceableObject>();

        if (objectToPlace.GetComponent<Turret>() != null)
        {
            objectToPlaceCost = objectToPlace.GetComponent<Turret>().GetCost();
            objectToPlace.GetComponent<Turret>().enabled = false;
        } else
        {
            objectToPlaceCost = 0;
        }
            
        objectToPlace.AddComponent<ObjectDrag>();
    }

    public bool InitializeObjectRNG(GameObject prefab, Vector3 position)
    {
        Vector3 positionSnap = SnapCoordinateToGrid(position);

        objectToPlaceRNG = Instantiate(prefab, positionSnap, Quaternion.identity);
        objectToPlaceRNG_PlaceableObjectScript = objectToPlaceRNG.GetComponent<PlaceableObject>();
        objectToPlaceRNG_PlaceableObjectScript.Setup();

        if (objectToPlaceRNG.GetComponent<Turret>() != null)
        {
            objectToPlaceRNG.GetComponent<Turret>().enabled = false;
        }

        objectToPlaceRNG.AddComponent<ObjectDrag>();
        objectToPlaceRNG.GetComponent<ObjectDrag>().enabled = false;

        if (!IsColideWithWhiteTile(objectToPlaceRNG_PlaceableObjectScript))
        {
            //Debug.Log("invalid RNG placement.");
            Destroy(objectToPlaceRNG);
            return false;
        } else
        {
        objectToPlaceRNG_PlaceableObjectScript.Place();
            Vector3Int start = gridLayout.WorldToCell(objectToPlaceRNG_PlaceableObjectScript.GetStartPosition()); 
            TakeArea(start, objectToPlaceRNG_PlaceableObjectScript.Size);
        if (objectToPlaceRNG.GetComponent<Turret>() != null)
        {
            //Debug.Log("RNG is enable");
            objectToPlaceRNG.GetComponent<Turret>().enabled = true;
        }

        return true;

        }
    }

    private bool IsColideWithWhiteTile(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(placeableObject.GetStartPosition()); //this one execute too fast in RNG object
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

    private bool IsInRange()
    {
        //this will be square range with middle being building range and buildingRange as length/2
        bool isBuildingXTooFar = (Mathf.Abs(objectToPlace.transform.position.x) <= buildingRange + humanKingdom.transform.position.x);
        bool isBuildingZTooFar = (Mathf.Abs(objectToPlace.transform.position.z) <= buildingRange + humanKingdom.transform.position.z);

        return isBuildingXTooFar && isBuildingZTooFar;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, whiteTile, start.x, start.y,
                        start.x + size.x, start.y + size.y);
    }

    #endregion
}

