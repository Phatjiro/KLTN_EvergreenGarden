using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum FarmMode
{
    None,
    Digging,
    Watering,
    PlantingGrass,
    PlantingCarrot,
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

    public static FarmMode currentMode = FarmMode.None;

    // Start is called before the first frame update
    void Start()
    {
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
                        break;

                    case FarmMode.Watering:
                        if (cellInFarmGround == tileToPlace_groundDigged)
                        {
                            tilemap_GroundWatered.SetTile(cellPos, tileToPlace_groundWatered);
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
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
