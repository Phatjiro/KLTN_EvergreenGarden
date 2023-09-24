using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum FarmMode
{
    None,
    Digging,
    Watering,
    PlatingGrass
}

public class FarmAction : MonoBehaviour
{
    [SerializeField]
    Tilemap tilemap_FarmGround;
    [SerializeField]
    Tilemap tilemap_GroundWatered;
    [SerializeField]
    TileBase tileToPlace_groundDigged;
    [SerializeField]
    TileBase tileToPlace_groundWatered;

    public static FarmMode currentMode = FarmMode.None;

    // Start is called before the first frame update
    void Start()
    {
        
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
                Vector3Int cellPos = tilemap_FarmGround.WorldToCell(touchWorldPos);

                // Get cell in other tilemap
                var cellInFarmGround = tilemap_FarmGround.GetTile(cellPos);
                var cellInGroundWatered = tilemap_GroundWatered.GetTile(cellPos);

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

                    case FarmMode.PlatingGrass:
                        if (cellInGroundWatered == tileToPlace_groundWatered)
                        {
                            tilemap_GroundWatered.SetTile(cellPos, null);
                        }
                        if (cellInFarmGround == tileToPlace_groundDigged)
                        {
                            tilemap_FarmGround.SetTile(cellPos, null);
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
