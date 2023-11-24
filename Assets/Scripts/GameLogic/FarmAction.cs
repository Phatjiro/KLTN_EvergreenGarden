using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public interface ReadDataCallback
{
    public void OnReadDataMapCompleted(string data);
    public void OnReadDataUserCompleted(string data); 

    public void OnReadDataAnimalCompleted(String data);
}

public enum FarmMode
{
    None,
    Digging,
    Watering,
    PlantingGrass,
    PlantingCarrot,
    PlantingCorn,
    PlantingRice,
    Gloving,
}

[Serializable]
public class AllTileMap
{
    [SerializeField]
    public Tilemap tilemap_FarmGround;
    [SerializeField]
    public Tilemap tilemap_GroundWatered;
    [SerializeField]
    public Tilemap tilemap_Planting;
    [SerializeField]
    public Tilemap tilemap_Farmable;
}

public class PlantTimeInformation
{
    public DateTime dateTime;
    public DateTime nextGrowTime;
    public ItemType itemType;
    public Vector3Int cellPos;
    public PlantTimeInformation(DateTime dateTime, ItemType itemType, Vector3Int cellPos, DateTime nextGrow)
    {
        this.dateTime = dateTime;
        this.itemType = itemType;
        this.cellPos = cellPos;
        this.nextGrowTime = nextGrow;
    }
}

public class FarmAction : MonoBehaviour, ReadDataCallback
{
    [SerializeField]
    AllTileMap allTileMap;
    [SerializeField]
    TileBase tileToPlace_groundDigged;
    [SerializeField]
    TileBase tileToPlace_groundWatered;

    // Carrot
    [SerializeField]
    List<TileBase> tilebaseCarrot;

    // Corn
    [SerializeField]
    List<TileBase> tileBaseCorn;

    // Rice
    [SerializeField]
    List<TileBase> tilebaseRice;

    List<PlantTimeInformation> lstPlantedTime;

    [SerializeField]
    Image imageNotification;
    [SerializeField]
    Text textMessageContent;

    // Manager Firebase Database
    [SerializeField]
    FirebaseReadData firebaseReadData;
    [SerializeField]
    FirebaseWriteData firebaseWriteData;

    [SerializeField]
    UserLoaderManager userLoaderManager;

    [SerializeField]
    MapLoaderManager mapLoaderManager;

    public static FarmMode currentMode = FarmMode.None;

    SoundButtonManager soundButtonManager;

    private void Awake()
    {
        // Init Map and User
        //userInGame = new User();
        lstPlantedTime = new List<PlantTimeInformation>();

//#if !UNITY_EDITOR
//        // Get user already login
//        firebaseUser = FirebaseAuth.DefaultInstance.CurrentUser;
//        Debug.Log(firebaseUser.UserId + " - " + firebaseUser.DisplayName);
//#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        soundButtonManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundButtonManager>();

        imageNotification.gameObject.SetActive(false);
        InvokeRepeating("UpdateMapDataToFirebase", 20, 10);
        InvokeRepeating("GrowPlant", 0, 1);
    }

    private void UpdateMapDataToFirebase()
    {
        if (!MapLoaderManager.isLoadedMap)
            return;
        Debug.Log("Update map");
        firebaseWriteData.WriteData("Maps/" + userLoaderManager.userInGame.id, mapLoaderManager.userMap.ToString());
    }

    public void AddPlantTime(DateTime currentDateTime, ItemType itemType, Vector3Int cellPos, DateTime nextGrowTime)
    {
        if (lstPlantedTime == null)
        { 
            lstPlantedTime = new List<PlantTimeInformation>();
        }
        lstPlantedTime.Add(new PlantTimeInformation(currentDateTime, itemType, cellPos, nextGrowTime));
    }
    private TileBase GetNextPlantTileBase(TileBase crr, ItemType itemType)
    {
        if (crr == null)
        {
            Debug.Log("Get next for null");
            return null;
        }
        Debug.Log("Get next for: " + crr.name);
        List<TileBase> searchList = new List<TileBase>();
        switch (itemType)
        {
            case ItemType.Carrot:
                searchList = this.tilebaseCarrot;
                break;
            case ItemType.Corn:
                searchList = this.tileBaseCorn;
                break;
            case ItemType.Rice:
                searchList = this.tilebaseRice;
                break;
            default:
                break;
        }

        for (int i = 0; i < searchList.Count; i++)
        {
            Debug.Log("Compare: " + searchList[i].name);
            if (searchList[i].name == crr.name)
            {
                if (i+1 < searchList.Count)
                {
                    Debug.Log("found next: " + searchList[i + 1].name);
                    return searchList[i+1];
                }
            }
        }
        return null;
    }

    private void GrowPlant()
    {
        if (this.lstPlantedTime != null && this.lstPlantedTime.Count > 0)
        {
            List<int> lstIndexRemove = new List<int>();
            int index = 0;
            foreach (var t in this.lstPlantedTime)
            {
                Vector3Int plantPos = t.cellPos;
                ItemType itemType = t.itemType;
                TileBase crrTileBase = allTileMap.tilemap_Planting.GetTile(plantPos);
                

                if (DateTime.Now > t.nextGrowTime)
                {
                    Debug.Log("pos: " + plantPos + " now: " + DateTime.Now);
                    Debug.Log("crr: " + crrTileBase.name);
                    TileBase nextTielBase = GetNextPlantTileBase(crrTileBase, itemType);
                    allTileMap.tilemap_Planting.SetTile(plantPos, nextTielBase);
                    t.nextGrowTime = DateTime.Now.AddSeconds(20);
                    if (GetNextPlantTileBase(nextTielBase, itemType) == null)
                    {
                        lstIndexRemove.Insert(0, index);
                    }
                }
                index++;
            }
            foreach (int id in lstIndexRemove)
            {
                this.lstPlantedTime.RemoveAt(id);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If user is clicking button UI -> return
        if (EventButtonManager.GetIsClickingButton()) return;
        
        if (currentMode == FarmMode.None)
        {
            return;
        }

        // Change the tilemap when click button
        // Action for farmer to build his farm
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
                Vector3Int cellPos = allTileMap.tilemap_FarmGround.WorldToCell(touchWorldPos);

                // Get cell in other tilemap
                var cellInFarmGround = allTileMap.tilemap_FarmGround.GetTile(cellPos);
                var cellInGroundWatered = allTileMap.tilemap_GroundWatered.GetTile(cellPos);
                var cellInPlanting = allTileMap.tilemap_Planting.GetTile(cellPos);

                // SwitchCase to active mode
                switch (currentMode)
                {
                    case FarmMode.None:
                        break;

                    case FarmMode.Digging:
                        if (allTileMap.tilemap_Farmable.GetTile(cellPos) != null)
                        {
                            soundButtonManager.PlaySFX(soundButtonManager.digging);
                            DiggingGround(cellPos);
                        }
                        break;
                    case FarmMode.Watering:
                        if (allTileMap.tilemap_Farmable.GetTile(cellPos) == tileToPlace_groundDigged)
                        {
                            soundButtonManager.PlaySFX(soundButtonManager.watering);
                            WateringGround(cellPos);
                        }
                        else
                        {
                            ShowNotification("Please water the dug soil bed", 2);
                        }
                        break;

                    case FarmMode.PlantingGrass:
                        if (cellInGroundWatered == tileToPlace_groundWatered)
                        {
                            allTileMap.tilemap_GroundWatered.SetTile(cellPos, null);
                        }
                        if (cellInFarmGround == tileToPlace_groundDigged)
                        {
                            allTileMap.tilemap_FarmGround.SetTile(cellPos, null);
                        }
                        break;

                    case FarmMode.PlantingCarrot:
                        if (cellInGroundWatered == tileToPlace_groundWatered && allTileMap.tilemap_Planting.GetTile(cellPos) == null)
                        {
                            soundButtonManager.PlaySFX(soundButtonManager.planting);
                            Plant(cellPos, ItemType.Carrot, 0);
                        }
                        else
                        {
                            ShowNotification("Please plant the dug wattered bed", 2);
                        }
                        break;

                    case FarmMode.PlantingCorn:
                        if (cellInGroundWatered == tileToPlace_groundWatered && allTileMap.tilemap_Planting.GetTile(cellPos) == null)
                        {
                            soundButtonManager.PlaySFX(soundButtonManager.planting);
                            Plant(cellPos, ItemType.Corn, 0);
                        }
                        else
                        {
                            ShowNotification("Please plant the dug wattered bed", 2);
                        }
                        break;

                    case FarmMode.PlantingRice:
                        if (cellInGroundWatered == tileToPlace_groundWatered && allTileMap.tilemap_Planting.GetTile(cellPos) == null)
                        {
                            soundButtonManager.PlaySFX(soundButtonManager.planting);
                            Plant(cellPos, ItemType.Rice, 0);
                        }
                        else
                        {
                            ShowNotification("Please plant the dug wattered bed", 2);
                        }
                        break;

                    case FarmMode.Gloving:
                        if (cellInPlanting == tilebaseCarrot[3])
                        {
                            soundButtonManager.PlaySFX(soundButtonManager.picking_up_plant);
                            Harvest(itemType: ItemType.Carrot, quantity: 1, cellPos);
                        }
                        if (cellInPlanting == tileBaseCorn[3])
                        {
                            soundButtonManager.PlaySFX(soundButtonManager.picking_up_plant);
                            Harvest(itemType: ItemType.Corn, quantity: 1, cellPos);
                        }
                        if (cellInPlanting == tilebaseRice[3])
                        {
                            soundButtonManager.PlaySFX(soundButtonManager.picking_up_plant);
                            Harvest(itemType: ItemType.Rice, quantity: 1, cellPos);
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public void DiggingGround(Vector3Int cellPos, bool isNeedAddToUserMap = true)
    {
        Debug.Log("Digging at: " + cellPos);
        allTileMap.tilemap_Farmable.SetTile(cellPos, tileToPlace_groundDigged);
        CellData cellData = new CellData(cellPos.x, cellPos.y, CellState.Digged);
        if (isNeedAddToUserMap)
            mapLoaderManager.userMap.AddCell(cellData);
    }

    public void WateringGround(Vector3Int cellPos, bool isNeedToAddToUserMap = true)
    {
        allTileMap.tilemap_GroundWatered.SetTile(cellPos, tileToPlace_groundWatered);
        if (isNeedToAddToUserMap)
            mapLoaderManager.userMap.AddCell(new CellData(cellPos.x, cellPos.y, CellState.Watered));
    }
    public void Plant(Vector3Int cellPos, ItemType itemType, int state, bool isNeedAddPlantTime = true, bool isNeddAddUserMap = true)
    {
        Debug.Log($"Plant:{itemType} - state:{state}");
        switch (itemType)
        {
            case ItemType.Carrot:
                allTileMap.tilemap_Planting.SetTile(cellPos, tilebaseCarrot[state]);
                Debug.Log("Set tile carrot: " + cellPos);
                if (isNeedAddPlantTime)
                    AddPlantTime(DateTime.Now, ItemType.Carrot, cellPos, DateTime.Now.AddSeconds(10));
                if (isNeddAddUserMap)
                    mapLoaderManager.userMap.AddCell(new CellData(cellPos.x, cellPos.y, CellState.Carrot));
                break;
            case ItemType.Corn:
                allTileMap.tilemap_Planting.SetTile(cellPos, tileBaseCorn[state]);
                Debug.Log("Set tile Corn: " + cellPos);
                if (isNeedAddPlantTime)
                    AddPlantTime(DateTime.Now, ItemType.Corn, cellPos, DateTime.Now.AddSeconds(10));
                if (isNeddAddUserMap)
                    mapLoaderManager.userMap.AddCell(new CellData(cellPos.x, cellPos.y, CellState.Corn));
                break;
            case ItemType.Rice:
                allTileMap.tilemap_Planting.SetTile(cellPos, tilebaseRice[state]);
                Debug.Log("Set tile rice: " + cellPos);
                if (isNeedAddPlantTime)
                    AddPlantTime(DateTime.Now, ItemType.Rice, cellPos, DateTime.Now.AddSeconds(10));
                if (isNeddAddUserMap)
                    mapLoaderManager.userMap.AddCell(new CellData(cellPos.x, cellPos.y, CellState.Rice));
                break;
        }
        
    }
    private void Harvest(ItemType itemType, int quantity, Vector3Int cellPos)
    {
        allTileMap.tilemap_FarmGround.SetTile(cellPos, null);
        allTileMap.tilemap_Planting.SetTile(cellPos, null);
        allTileMap.tilemap_GroundWatered.SetTile(cellPos, null);
        ItemInBag item = new ItemInBag(itemType, quantity);

        // Add item to user bag
        userLoaderManager.userInGame.AddItemToBagAndLoadUI(itemType, quantity);
        //userLoaderManager.userInGame.ShowBag();
        mapLoaderManager.userMap.SetState(cellPos, CellState.None);
    }



    public void OnReadDataMapCompleted(string data)
    {
        return;
    }

    public void OnReadDataUserCompleted(string data)
    {
        //Debug.Log("User data: " + data);
        //userInGame = JsonConvert.DeserializeObject<User>(data);
        //Debug.Log("Load user successful: " + userInGame.ToString());
        return;
    }

    private void ShowNotification(string mess, int time)
    {
        if (NotificationManager.instance == null) return;
        NotificationManager.instance.ShowNotification(mess, time);
    }

    public void OnReadDataAnimalCompleted(string data)
    {
        
    }
}
