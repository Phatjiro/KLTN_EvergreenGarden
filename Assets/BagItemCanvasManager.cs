using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.UI;

public class BagItemCanvasManager : MonoBehaviour, ICell
{
    [SerializeField]
    Image iconDisplay;

    [SerializeField]
    Text quantityDisplay;

    //Model
    private ItemInBag _itemInfo;
    private int _cellIndex;

    public void SetIcon(Sprite sprite)
    {
        iconDisplay.sprite = sprite;
        iconDisplay.color = Color.white;
    }
    public void SetQuantity(int quantity)
    {
        Debug.Log("Set quantity: " + quantity);
        this.quantityDisplay.text = "x"+quantity+"";
    }

    public void ConfigureCell(ItemInBag item, int index)
    {
        _cellIndex = index;
        _itemInfo = item;

        SetQuantity(item.quantity);
        SetIcon(ItemInformationManager._instance.GetIcon(item.type));
    }
}
