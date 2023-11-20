using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.UI;

public class BagItemCanvasManager : MonoBehaviour, ICell
{
    [SerializeField]
    Image iconDisplay;

    [SerializeField]
    Text quantityDisplay;

    [SerializeField]
    Button buttonBackground;

    GameObject canvasObject;
    GameObject sellBoardObject;

    //Model
    private ItemInBag _itemInfo;
    private int _cellIndex;

    private void Awake()
    {
        buttonBackground.onClick.AddListener(OpenSellBoard);

        canvasObject = GameObject.FindGameObjectWithTag("Canvas");
        sellBoardObject = canvasObject.transform.Find("SellBoard").gameObject;
    }

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

    public void OpenSellBoard()
    {
        sellBoardObject.GetComponent<SellBoardManager>().OpenCart(_itemInfo.type);
    }
}
