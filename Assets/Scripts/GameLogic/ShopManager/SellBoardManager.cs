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

    SoundButtonManager soundButtonManager;

    private void Awake()
    {
        buttonMinus.onClick.AddListener(MinusItem);
        buttonPlus.onClick.AddListener(PlusItem);
        buttonSubmit.onClick.AddListener(SubmitItem);
        buttonSellAll.onClick.AddListener(SellAllChooseItem);
    }

    // Start is called before the first frame update
    void Start()
    {
        soundButtonManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundButtonManager>();
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

            case ItemType.Rice:
                SetUpItemSellBoard(ItemInformationManager._instance.GetIcon(ItemType.Rice), currentQuantity, 4);
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
            soundButtonManager.PlaySFX(soundButtonManager.clickButton);
            currentQuantity -= 1;
            SetUpSellBoard();
        }
        else
        {
            soundButtonManager.PlaySFX(soundButtonManager.failed);
            NotificationManager.instance.ShowNotification("The quantity of items cannot be less than 1", 4);
            return;
        }
    }

    public void PlusItem()
    {
        for (int i = 0; i < UserLoaderManager.userInGame.userBag.GetLength(); i++)
        {
            if (UserLoaderManager.userInGame.userBag.lstItem[i].type == itemChoose)
            {
                if (currentQuantity < UserLoaderManager.userInGame.userBag.lstItem[i].quantity)
                {
                    soundButtonManager.PlaySFX(soundButtonManager.clickButton);
                    currentQuantity += 1;
                    SetUpSellBoard();
                    return;
                }
                else
                {
                    soundButtonManager.PlaySFX(soundButtonManager.failed);
                    NotificationManager.instance.ShowNotification("Can't sell more items than you own", 4);
                }
            }
        }
    }

    public void SubmitItem()
    {
        soundButtonManager.PlaySFX(soundButtonManager.success);
        Debug.Log("SubmitItem");
        UserLoaderManager.userInGame.SellItemAndLoadUI(itemChoose, currentQuantity);
        
        UserLoaderManager.userInGame.gold += int.Parse(textTotal.text);
        firebaseWriteData.WriteData("Users/" + UserLoaderManager.userInGame.id, UserLoaderManager.userInGame.ToString());
        gameObject.SetActive(false);
    }

    public void SellAllChooseItem()
    {
        soundButtonManager.PlaySFX(soundButtonManager.success);
        int quantityOfChooseItem = 0;
        for (int i = 0; i < UserLoaderManager.userInGame.userBag.GetLength(); i++)
        {
            if (UserLoaderManager.userInGame.userBag.lstItem[i].type == itemChoose)
            {
                quantityOfChooseItem = UserLoaderManager.userInGame.userBag.lstItem[i].quantity;
                UserLoaderManager.userInGame.SellItemAndLoadUI(itemChoose, quantityOfChooseItem);
                break;
            }
        }
        UserLoaderManager.userInGame.gold += (int.Parse(textPrice.text) * quantityOfChooseItem);
        firebaseWriteData.WriteData("Users/" + UserLoaderManager.userInGame.id, UserLoaderManager.userInGame.ToString());
        gameObject.SetActive(false);
    }
}
