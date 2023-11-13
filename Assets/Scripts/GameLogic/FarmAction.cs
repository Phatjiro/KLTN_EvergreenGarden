using DG.Tweening;
using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public interface ReadDataCallback
{
    public void OnReadDataMapCompleted(string data);
    public void OnReadDataUserCompleted(string data); 
}

public enum FarmMode
{
    None,
    Digging,
    Watering,
    PlantingGrass,
    PlantingCarrot,
    Gloving,
    PlantingCorn,
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

    List<PlantTimeInformation> lstPlantedTime;
    List<int> lstPlantState;

    [SerializeField]
    Image imageNotification;
    [SerializeField]
    Text textMessageContent;

    // Manager Firebase Database
    [SerializeField]
    FirebaseReadData firebaseReadData;
    [SerializeField]
    FirebaseWriteData firebaseWriteData;

    FirebaseUser firebaseUser;
    User userInGame;

    public static FarmMode currentMode = FarmMode.None;

    private void Awake()
    {
        // Init Map and User
        userInGame = new User();
        lstPlantedTime = new List<PlantTimeInformation>();
        lstPlantState = new List<int>();

#if !UNITY_EDITOR
        // Get user already login
        firebaseUser = FirebaseAuth.DefaultInstance.CurrentUser;
        Debug.Log(firebaseUser.UserId + " - " + firebaseUser.DisplayName);
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        if (firebaseReadData == null)
        {
            Debug.LogError("FirebaseReadData component not found!");
        }
        if (firebaseWriteData == null)
        {
            Debug.LogError("FirebaseWriteData component not found!");
        }
        else
        {
            if (firebaseUser != null)
            {
                // Read data map
                firebaseReadData.ReadData("Map", this, ReadDataType.Map);

                // Read data user
                firebaseReadData.ReadData("Users/" + firebaseUser.UserId, this, ReadDataType.User);
            }
        }

        imageNotification.gameObject.SetActive(false);
        InvokeRepeating("UpdateMapDataToFirebase", 1, 10);
        InvokeRepeating("GrowPlant", 0, 1);
    }

    private void UpdateMapDataToFirebase()
    {
        Debug.Log("Update map");
        firebaseWriteData.WriteData("Users/" + userInGame.id, userInGame.ToString());
    }

    private void AddPlantTime(DateTime currentDateTime, ItemType itemType, Vector3Int cellPos, DateTime nextGrowTime)
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
                    t.nextGrowTime = DateTime.Now.AddSeconds(10);
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
                            Debug.Log("Digging at: " + cellPos);
                            allTileMap.tilemap_Farmable.SetTile(cellPos, tileToPlace_groundDigged);
                            CellData cellData = new CellData(cellPos.x, cellPos.y, CellState.Digged);
                            userInGame.userMap.AddCell(cellData);
                        }
                        break;
                    case FarmMode.Watering:
                        if (allTileMap.tilemap_Farmable.GetTile(cellPos) == tileToPlace_groundDigged)
                        {
                            allTileMap.tilemap_GroundWatered.SetTile(cellPos, tileToPlace_groundWatered);
                            userInGame.userMap.AddCell(new CellData(cellPos.x, cellPos.y, CellState.Watered));
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
                            allTileMap.tilemap_Planting.SetTile(cellPos, tilebaseCarrot[0]);
                            Debug.Log("Set tile carrot: " + cellPos);
                            AddPlantTime(DateTime.Now, ItemType.Carrot, cellPos, DateTime.Now.AddSeconds(10));
                            userInGame.userMap.AddCell(new CellData(cellPos.x, cellPos.y, CellState.Carrot1));
                        }
                        else
                        {
                            ShowNotification("Please plant the dug wattered bed", 2);
                        }
                        break;

                    case FarmMode.PlantingCorn:
                        if (cellInGroundWatered == tileToPlace_groundWatered && allTileMap.tilemap_Planting.GetTile(cellPos) == null)
                        {
                            allTileMap.tilemap_Planting.SetTile(cellPos, tileBaseCorn[0]);
                            AddPlantTime(DateTime.Now, ItemType.Corn, cellPos, DateTime.Now.AddSeconds(10));
                            userInGame.userMap.AddCell(new CellData(cellPos.x, cellPos.y, CellState.Corn1));
                        }
                        else
                        {
                            ShowNotification("Please plant the dug wattered bed", 2);
                        }
                        break;
                    case FarmMode.Gloving:
                        if (cellInPlanting == tilebaseCarrot[3])
                        {
                            Harvest(itemType: ItemType.Carrot, quantity: 1, cellPos);
                        }
                        if (cellInPlanting == tileBaseCorn[3])
                        {
                            Harvest(itemType: ItemType.Corn, quantity: 1, cellPos);
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private void Harvest(ItemType itemType, int quantity, Vector3Int cellPos)
    {
        allTileMap.tilemap_Planting.SetTile(cellPos, null);
        allTileMap.tilemap_GroundWatered.SetTile(cellPos, null);
        ItemInBag item = new ItemInBag(itemType, quantity);

        // Add item to user bag
        userInGame.AddItemToBag(item, quantity);
        userInGame.ShowBag();
    }

    private void ShowNotification(string messContent, int timeShowNotification)
    {
        imageNotification.gameObject.SetActive(true);
        textMessageContent.text = messContent;
        imageNotification.GetComponent<RectTransform>().DOAnchorPosY(-100, 1);
        imageNotification.DOFade(1, timeShowNotification).OnComplete(() =>
        {
            imageNotification.GetComponent<RectTransform>().DOAnchorPosY(100, 1);
        });
    }

    public void OnReadDataMapCompleted(string data)
    {
        return;
    }

    public void OnReadDataUserCompleted(string data)
    {
        Debug.Log("User data: " + data);
        userInGame = JsonConvert.DeserializeObject<User>(data);
        Debug.Log("Load user successful: " + userInGame.ToString());
    }
}
