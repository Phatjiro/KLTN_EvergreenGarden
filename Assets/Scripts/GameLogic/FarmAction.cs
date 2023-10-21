using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum FarmMode
{
    None,
    Digging,
    Watering,
    PlantingGrass,
    PlantingCarrot,
    Gloving,
}

public class FarmAction : MonoBehaviour
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

    public Map map;

    public static FarmMode currentMode = FarmMode.None;

    // Start is called before the first frame update
    void Start()
    {
        map = new Map();
        map.ReadFileTxt();
        LoadMap(map);

        imageNotification.gameObject.SetActive(false);

        lstPlantedTime = new List<DateTime>();
        lstCellPos = new List<Vector3Int>();
        lstPlantState = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        // If user is clicking button UI -> return
        if (EventButtonManager.GetIsClickingButton()) return;

        if (this.lstPlantedTime != null && this.lstPlantedTime.Count > 0)
        {
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

                }
                index++;
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
                        map.AddCell(cellData);
                        map.ShowMap();
                        //tilemap_GroundWatered.SetTile(cellPos, tileToPlace_carrot_04);
                        map.ExportFileTxt();
                        break;

                    case FarmMode.Watering:
                        if (cellInFarmGround == tileToPlace_groundDigged)
                        {
                            tilemap_GroundWatered.SetTile(cellPos, tileToPlace_groundWatered);
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
                            lstPlantedTime.Add(DateTime.Now);
                            this.lstCellPos.Add(cellPos);
                            this.lstPlantState.Add(0);
                        }
                        else
                        {
                            ShowNotification("Please plant the dug wattered bed", 2);
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

    private void CellDataToTiseBase(CellData cellData)
    {
        TileBase tileToPlace = null;
        Tilemap tilemap = null;
        Debug.Log(cellData);
        switch (cellData.cellState)
        {
            case CellState.None:
                break;
            case CellState.Ground:
                break;
            case CellState.Digged:
                Debug.Log("Enter digged");
                tileToPlace = tileToPlace_groundDigged;
                tilemap = tilemap_FarmGround;
                ApplyCellDataToTilemap(cellData, tilemap, tileToPlace);
                break;
            case CellState.Watered:
                tileToPlace = tileToPlace_groundWatered;
                tilemap = tilemap_GroundWatered;
                ApplyCellDataToTilemap(cellData, tilemap, tileToPlace);
                break;
            case CellState.Carrot:
                break;
            default:
                break;
        }
    }

    private void ApplyCellDataToTilemap(CellData cellData, Tilemap tilemap, TileBase tileBase)
    {
        Vector3Int cellPos = new Vector3Int(cellData.x, cellData.y, 0);
        tilemap.SetTile(cellPos, tileBase);
    }

    private void LoadMap(Map map)
    {
        for (int i = 0; i < map.GetLength(); i++)
        {
            CellDataToTiseBase(map.lstCell[i]);
        }
    }    
}
