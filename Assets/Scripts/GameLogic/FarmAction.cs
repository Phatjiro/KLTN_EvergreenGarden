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

public class FarmAction : MonoBehaviour, ReadDataCallback
{
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

    // Carrot
    [SerializeField]
    TileBase tileToPlace_carrot_01;
    [SerializeField]
    TileBase tileToPlace_carrot_02;
    [SerializeField]
    TileBase tileToPlace_carrot_03;
    [SerializeField]
    TileBase tileToPlace_carrot_04;

    List<DateTime> lstPlantedTime;
    List<Vector3Int> lstCellPos;
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
        lstPlantedTime = new List<DateTime>();
        lstCellPos = new List<Vector3Int>();
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
    }

    private void UpdateMapDataToFirebase()
    {
        Debug.Log("Update map");
        firebaseWriteData.WriteData("Users/" + userInGame.id, userInGame.ToString());
    }

    private void AddPlantTime(DateTime currentDateTime)
    {
        if (lstPlantedTime == null)
        { 
            lstPlantedTime = new List<DateTime>();
        }
        lstPlantedTime.Add(currentDateTime);
    }
    private void AddCellPos(Vector3Int cellPos)
    {
        if (lstCellPos == null)
        {
            lstCellPos = new List<Vector3Int>();
        }
        lstCellPos.Add(cellPos);
    }
    private void AddPlantState(int plants)
    {
        if (lstPlantState == null)
        {
            lstPlantState= new List<int>();
        }
        lstPlantState.Add(plants);
    }

    // Update is called once per frame
    void Update()
    {
        // If user is clicking button UI -> return
        if (EventButtonManager.GetIsClickingButton()) return;

        if (this.lstPlantedTime != null && this.lstPlantedTime.Count > 0)
        {
            List<int> lstIndexRemove = new List<int>();
            int index = 0;
            foreach (var t in this.lstPlantedTime)
            {
                double secondAfterPlantedTime = DateTime.Now.Subtract(t).TotalSeconds;
                if (secondAfterPlantedTime >= 10 && secondAfterPlantedTime < 20)
                {
                    tilemap_Planting.SetTile(lstCellPos[index], tileToPlace_carrot_02);
                }
                else if (secondAfterPlantedTime >= 20 && secondAfterPlantedTime < 30)
                {
                    tilemap_Planting.SetTile(lstCellPos[index], tileToPlace_carrot_03);
                }
                else if (secondAfterPlantedTime >= 30)
                {
                    tilemap_Planting.SetTile(lstCellPos[index], tileToPlace_carrot_04);
                    lstIndexRemove.Insert(0, index);
                }
                index++;
            }
            foreach (int id in lstIndexRemove)
            {
                this.lstPlantedTime.RemoveAt(id);
                this.lstCellPos.RemoveAt(id);
            }
        }
        
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
                Vector3Int cellPos = tilemap_FarmGround.WorldToCell(touchWorldPos);

                // Get cell in other tilemap
                var cellInFarmGround = tilemap_FarmGround.GetTile(cellPos);
                var cellInGroundWatered = tilemap_GroundWatered.GetTile(cellPos);
                var cellInPlanting = tilemap_Planting.GetTile(cellPos);

                // SwitchCase to active mode
                switch (currentMode)
                {
                    case FarmMode.None:
                        break;

                    case FarmMode.Digging:
                        tilemap_FarmGround.SetTile(cellPos, tileToPlace_groundDigged);
                        CellData cellData = new CellData(cellPos.x, cellPos.y, CellState.Digged);
                        userInGame.userMap.AddCell(cellData);
                        break;

                    case FarmMode.Watering:
                        if (cellInFarmGround == tileToPlace_groundDigged)
                        {
                            tilemap_GroundWatered.SetTile(cellPos, tileToPlace_groundWatered);
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
                            tilemap_GroundWatered.SetTile(cellPos, null);
                        }
                        if (cellInFarmGround == tileToPlace_groundDigged)
                        {
                            tilemap_FarmGround.SetTile(cellPos, null);
                        }
                        break;

                    case FarmMode.PlantingCarrot:
                        if (cellInGroundWatered == tileToPlace_groundWatered)
                        {
                            tilemap_Planting.SetTile(cellPos, tileToPlace_carrot_01);
                            AddPlantTime(DateTime.Now);
                            AddCellPos(cellPos);
                            AddPlantState(0);
                        }
                        else
                        {
                            ShowNotification("Please plant the dug wattered bed", 2);
                        }
                        break;

                    case FarmMode.Gloving:
                        if (cellInPlanting == tileToPlace_carrot_04)
                        {
                            tilemap_Planting.SetTile(cellPos, null);
                            tilemap_GroundWatered.SetTile(cellPos, null);
                            ItemInBag carrotAdd = new ItemInBag(ItemType.Carrot, 1);

                            // Add item to user bag
                            userInGame.AddItemToBag(carrotAdd);

                            userInGame.ShowBag();
                            userInGame.AddItemToBag(ItemType.Carrot, 1);
                        }
                        break;

                    default:
                        break;
                }
            }
        }
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
