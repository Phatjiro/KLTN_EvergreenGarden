using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLoaderManager : MonoBehaviour, ReadDataCallback
{
    public static bool isLoadedMap = false;
    FirebaseUser firebaseUser;

    [SerializeField]
    FirebaseReadData firebaseReadData;

    public Map userMap;

    [SerializeField]
    Tilemap tilemap_FarmGround;
    [SerializeField]
    Tilemap tilemap_GroundWatered;
    [SerializeField]
    Tilemap tilemap_Planting;

    [SerializeField]
    TileBase tileToPlace_groundDigged;
    [SerializeField]
    TileBase tileToPlace_groundWatered;


    [SerializeField]
    FarmAction farmAction;

    private void Awake()
    {
        firebaseUser = FirebaseAuth.DefaultInstance.CurrentUser;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (firebaseUser != null)
        {
            firebaseReadData.ReadData("Maps/" + firebaseUser.UserId, this, ReadDataType.Map);
            //FirebaseDatabase.DefaultInstance.GetReference("Maps/" + firebaseUser.UserId).ValueChanged += OnDataChanged;
        }

        Debug.Log("PHAT TEST (MAPJSON): " + FriendItemCanvasManager.MapJsonWaitToLoad);
        Debug.Log("PHAT TEST (isNEEDLOAD): " + FriendItemCanvasManager.isNeedToLoadVisitMap);

        if (FriendItemCanvasManager.isNeedToLoadVisitMap)
        {
            FriendItemCanvasManager.isNeedToLoadVisitMap = false;
            Debug.Log("PHAT TEST (MAPJSON): " + FriendItemCanvasManager.MapJsonWaitToLoad);
            LoadMap(JsonConvert.DeserializeObject <Map>(FriendItemCanvasManager.MapJsonWaitToLoad));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDataChanged(object sender, ValueChangedEventArgs args)
    {
       /* if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        firebaseReadData.ReadData("Maps/" + firebaseUser.UserId, this, ReadDataType.Map);*/
    }

    private void ApplyCellDataToTilemap(CellData cellData, Tilemap tilemap, TileBase tileBase)
    {
        Vector3Int cellPos = new Vector3Int(cellData.x, cellData.y, 0);
        tilemap.SetTile(cellPos, tileBase);
    }

    private void CellDataToTiseBase(CellData cellData)
    {
        Debug.Log("-----------------------------------------------");
        Debug.Log("Start to set cellData: " + cellData.cellState);
        TileBase tileToPlace = null;
        Tilemap tilemap = null;
        Vector3Int cellPos = new Vector3Int(cellData.x, cellData.y, 0);

        // Data to check state plant
        DateTime platnedTime = cellData.dateTime;
        float secondPassed = (float)DateTime.Now.Subtract(platnedTime).TotalSeconds;
        bool isNeedAddPlantTime = true;

        switch (cellData.cellState)
        {
            case CellState.None:
                break;
            case CellState.Ground:
                break;
            case CellState.Digged:
                Debug.Log("Set state Dig");
                farmAction.DiggingGround(cellPos, false);
                break;
            case CellState.Watered:
                Debug.Log("Set state Water");
                farmAction.DiggingGround(cellPos, false);
                farmAction.WateringGround(cellPos, false);
                break;
            case CellState.Carrot:
                Debug.Log("Set state Carrot");
                farmAction.DiggingGround(cellPos, false);
                farmAction.WateringGround(cellPos, false);

                if (secondPassed > 60)
                {
                    //Carrot4
                    farmAction.Plant(cellPos, ItemType.Carrot, 3, false, false);
                    isNeedAddPlantTime = false;
                }
                else if (secondPassed > 40)
                {
                    //Carrot3
                    farmAction.Plant(cellPos, ItemType.Carrot, 2, false, false);
                } else if (secondPassed > 20)
                {
                    //Carrot2
                    farmAction.Plant(cellPos, ItemType.Carrot, 1, false, false);
                }
                else
                {
                    //Carrot1
                    farmAction.Plant(cellPos, ItemType.Carrot, 0, false, false);
                }
                if (isNeedAddPlantTime)
                    farmAction.AddPlantTime(platnedTime, ItemType.Carrot, cellPos, DateTime.Now.AddSeconds(20));
                break;
            case CellState.Corn:
                Debug.Log("Set state Corn");
                farmAction.DiggingGround(cellPos, false);
                farmAction.WateringGround(cellPos, false);

                if (secondPassed > 60)
                {
                    //Corn4
                    farmAction.Plant(cellPos, ItemType.Corn, 3, false, false);
                    isNeedAddPlantTime = false;
                }
                else if (secondPassed > 40)
                {
                    //Corn3
                    farmAction.Plant(cellPos, ItemType.Corn, 2, false, false);
                }
                else if (secondPassed > 20)
                {
                    //Corn2
                    farmAction.Plant(cellPos, ItemType.Corn, 1, false, false);
                }
                else
                {
                    //Corn1
                    farmAction.Plant(cellPos, ItemType.Corn, 0, false, false);
                }
                if (isNeedAddPlantTime)
                    farmAction.AddPlantTime(platnedTime, ItemType.Corn, cellPos, DateTime.Now.AddSeconds(20));
                break;
            case CellState.Rice:
                Debug.Log("Set state Rice");
                farmAction.DiggingGround(cellPos, false);
                farmAction.WateringGround(cellPos, false);

                if (secondPassed > 60)
                {
                    //Rice4
                    farmAction.Plant(cellPos, ItemType.Rice, 3, false, false);
                    isNeedAddPlantTime = false;
                }
                else if (secondPassed > 40)
                {
                    //Rice3
                    farmAction.Plant(cellPos, ItemType.Rice, 2, false, false);
                }
                else if (secondPassed > 20)
                {
                    //Rice2
                    farmAction.Plant(cellPos, ItemType.Rice, 1, false, false);
                }
                else
                {
                    //Rice1
                    farmAction.Plant(cellPos, ItemType.Rice, 0, false, false);
                }
                if (isNeedAddPlantTime)
                    farmAction.AddPlantTime(platnedTime, ItemType.Rice, cellPos, DateTime.Now.AddSeconds(20));
                break;
            default:
                break;
        }
    }

    public void LoadMap(Map map)
    {
        Debug.Log("MapLoaderManager - LoadMap: " + map.GetLength());
        for (int i = 0; i < map.GetLength(); i++)
        {
            CellDataToTiseBase(map.lstCell[i]);
        }
    }

    public void ClearMap()
    {
        for (int x = tilemap_FarmGround.cellBounds.min.x; x < tilemap_FarmGround.cellBounds.max.x; x++)
        {
            for (int y = tilemap_FarmGround.cellBounds.min.y; y < tilemap_FarmGround.cellBounds.max.y; y++)
            {
                tilemap_FarmGround.SetTile(new Vector3Int(x, y), null);
            }

        }
    }

    public void OnReadDataMapCompleted(string data)
    {
        if (isLoadedMap)
        {
            Debug.Log("Map loaded -> do not need load again");
            return;
        }
        Debug.Log("MapLoader Data: " + data);
        userMap = JsonConvert.DeserializeObject<Map>(data);
        LoadMap(userMap);
        isLoadedMap = true;
    }

    public void OnReadDataUserCompleted(string data)
    {
        return;
    }

    public void OnReadDataAnimalCompleted(string data)
    {
       
    }

    public void OnReadDataAllUserCompleted(List<string> data)
    { 
        
    }
}
