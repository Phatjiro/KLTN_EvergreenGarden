using UnityEngine;
using UnityEngine.UI;

public class SellBoardManager : MonoBehaviour
{
    [SerializeField]
    Image imageItem;

    [SerializeField]
    Text textQuantity;
    [SerializeField]
    Text textPrice;
    [SerializeField]
    Text textTotal;

    [SerializeField]
    Button buttonMinus;
    [SerializeField]
    Button buttonPlus;
    [SerializeField]
    Button buttonSellAll;
    [SerializeField]
    Button buttonSubmit;

    ItemType itemChoose;
    int currentQuantity = 1;

    [SerializeField]
    FirebaseWriteData firebaseWriteData;
    [SerializeField]
    UserLoaderManager userLoaderManager;

    private void Awake()
    {
        buttonMinus.onClick.AddListener(MinusItem);
        buttonPlus.onClick.AddListener(PlusItem);
        buttonSubmit.onClick.AddListener(SubmitItem);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpItemSellBoard(Sprite itemSprite, int itemQuantity, int itemPrice)
    {
        imageItem.sprite = itemSprite;
        textQuantity.text = itemQuantity.ToString();
        textPrice.text = itemPrice.ToString();
        textTotal.text = (itemPrice * itemQuantity).ToString();
    }

    public void SetUpSellBoard()
    {
        switch (itemChoose)
        {
            case ItemType.Carrot:
                SetUpItemSellBoard(ItemInformationManager._instance.GetIcon(ItemType.Carrot), currentQuantity, 8);
                break;

            case ItemType.Corn:
                SetUpItemSellBoard(ItemInformationManager._instance.GetIcon(ItemType.Corn), currentQuantity, 16);
                break;

            default:
                break;
        }
    }

    public void OpenCart(ItemType item)
    {
        if (TestBag.isAllowToSell == true)
        {
            itemChoose = item;
            currentQuantity = 1;
            gameObject.SetActive(true);
            SetUpSellBoard();
        }
    }

    public void MinusItem()
    {
        if (currentQuantity > 1)
        {
            currentQuantity -= 1;
            SetUpSellBoard();
        }
        else
        {
            NotificationManager.instance.ShowNotification("The quantity of items cannot be less than 1", 4);
            return;
        }
    }

    public void PlusItem()
    {
        for (int i = 0; i < userLoaderManager.userInGame.userBag.GetLength(); i++)
        {
            if (userLoaderManager.userInGame.userBag.lstItem[i].type == itemChoose)
            {
                if (currentQuantity < userLoaderManager.userInGame.userBag.lstItem[i].quantity)
                {
                    currentQuantity += 1;
                    SetUpSellBoard();
                    return;
                }
                else
                {
                    NotificationManager.instance.ShowNotification("Can't sell more items than you own", 4);
                }
            }
        }
    }

    public void SubmitItem()
    {
        for (int i = 0; i < userLoaderManager.userInGame.userBag.GetLength(); i++)
        {
            if (userLoaderManager.userInGame.userBag.lstItem[i].type == itemChoose)
            {
                userLoaderManager.userInGame.userBag.lstItem[i].quantity -= currentQuantity;
            }    
        }
        userLoaderManager.userInGame.gold += int.Parse(textTotal.text);
        firebaseWriteData.WriteData("Users/" + userLoaderManager.userInGame.id, userLoaderManager.userInGame.ToString());
        gameObject.SetActive(false);

        BagItemLoader loader = BagItemLoader.instance;
        if (loader != null)
        {
            Debug.Log("Vao load UI Bag");
            loader.ReloadUI();
        }
    }
}
