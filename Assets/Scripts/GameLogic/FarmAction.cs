using UnityEngine;
using UnityEngine.Tilemaps;

public enum FarmMode
{
    None,
    Digging,
    Watering
}

public class FarmAction : MonoBehaviour
{
    [SerializeField]
    Tilemap tilemap_Ground;
    [SerializeField]
    Tilemap Tilemap_FarmGround;
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
                Vector3Int cellPos = tilemap_Ground.WorldToCell(touchWorldPos);

                if (currentMode == FarmMode.Digging)
                {
                    tilemap_Ground.SetTile(cellPos, tileToPlace_groundDigged);
                }
                else if (currentMode == FarmMode.Watering)
                {
                    var cell = tilemap_Ground.GetTile(cellPos);
                    if (cell == tileToPlace_groundDigged)
                    {
                        Tilemap_FarmGround.SetTile(cellPos, tileToPlace_groundWatered);
                    }
                }
            }
        }
    }
}
