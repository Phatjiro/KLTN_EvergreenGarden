using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLoaderManager : MonoBehaviour, ReadDataCallback
{
    FirebaseUser firebaseUser;

    [SerializeField]
    FirebaseReadData firebaseReadData;

    Map userMap;

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

    private void Awake()
    {
        firebaseUser = FirebaseAuth.DefaultInstance.CurrentUser;
        userMap = new Map();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (firebaseUser != null)
        {
            firebaseReadData.ReadData("Users/" + firebaseUser.UserId, this, ReadDataType.User);
            FirebaseDatabase.DefaultInstance.GetReference("Users/" + firebaseUser.UserId).ValueChanged += OnDataChanged;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDataChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        firebaseReadData.ReadData("Users/" + firebaseUser.UserId, this, ReadDataType.User);
    }

    private void ApplyCellDataToTilemap(CellData cellData, Tilemap tilemap, TileBase tileBase)
    {
        Vector3Int cellPos = new Vector3Int(cellData.x, cellData.y, 0);
        tilemap.SetTile(cellPos, tileBase);
    }

    private void CellDataToTiseBase(CellData cellData)
    {
        TileBase tileToPlace = null;
        Tilemap tilemap = null;
        switch (cellData.cellState)
        {
            case CellState.None:
                break;
            case CellState.Ground:
                break;
            case CellState.Digged:
                tileToPlace = tileToPlace_groundDigged;
                tilemap = tilemap_FarmGround;
                Vector3Int cellPos = new Vector3Int(cellData.x, cellData.y, 0);
                ApplyCellDataToTilemap(cellData, tilemap, tileToPlace);
                break;
            case CellState.Watered:
                tileToPlace = tileToPlace_groundWatered;
                tilemap = tilemap_GroundWatered;
                ApplyCellDataToTilemap(cellData, tilemap, tileToPlace);
                break;
            case CellState.Carrot1:
                break;
            default:
                break;
        }
    }

    private void LoadMap(Map map)
    {
        Debug.Log("Map length: " + map.GetLength());
        for (int i = 0; i < map.GetLength(); i++)
        {
            CellDataToTiseBase(map.lstCell[i]);
        }
    }

    public void OnReadDataMapCompleted(string data)
    {
        return;
    }

    public void OnReadDataUserCompleted(string data)
    {
        Debug.Log("MapLoader Data: " + data);
        User userInGame = JsonConvert.DeserializeObject<User>(data);
        userMap = userInGame.userMap;
        LoadMap(userMap);
    }
}
