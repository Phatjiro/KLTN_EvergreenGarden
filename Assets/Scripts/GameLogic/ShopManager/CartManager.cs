using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CartManager : MonoBehaviour
{
    [SerializeField]
    bool isTestMode = false;

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
    Button buttonSubmit;

    ItemType itemChoose;
    int currentQuantity = 1;

    [SerializeField]
    FirebaseWriteData firebaseWriteData;
    [SerializeField]
    UserLoaderManager userLoaderManager;

    [SerializeField]
    AnimalManager animalManager;

    // Sprite
    [SerializeField]
    Sprite spriteChicken;
    [SerializeField]
    Sprite spritePig;
    [SerializeField]
    Sprite spriteCow;

    [SerializeField]
    SoundButtonManager soundButtonManager;

    private void Awake()
    {
        buttonMinus.onClick.AddListener(MinusItem);
        buttonPlus.onClick.AddListener(PlusItem);
        buttonSubmit.onClick.AddListener(() =>
        {
            SubmitBuyItem(ShopTypeManager.crrItemType);
        });
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUpItemCart(Sprite itemSprite, int itemQuantity, int itemPrice)
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
                SetUpItemCart(ItemInformationManager._instance.GetIcon(ItemType.Carrot), currentQuantity, 8);
                break;

            case ItemType.Corn:
                SetUpItemCart(ItemInformationManager._instance.GetIcon(ItemType.Corn), currentQuantity, 16);
                break;

            case ItemType.Rice:
                SetUpItemCart(ItemInformationManager._instance.GetIcon(ItemType.Rice), currentQuantity, 4);
                break;

            case ItemType.Chicken:
                int price = 50;
                if (isTestMode)
                    price = 0;
                SetUpItemCart(spriteChicken, currentQuantity, price);
                break;

            case ItemType.Pig:
                price = 100;
                if (isTestMode)
                    price = 0;
                SetUpItemCart(spritePig, currentQuantity, price);
                break;

            case ItemType.Cow:
                price = 150;
                if (isTestMode)
                    price = 0;
                SetUpItemCart(spriteCow, currentQuantity, price);
                break;

            default:
                break;
        }
    }

    public void OpenCart(ItemType item)
    {
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        itemChoose = item;
        currentQuantity = 1;
        gameObject.SetActive(true);
        SetUpSellBoard();
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
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        currentQuantity += 1;
        SetUpSellBoard();
    }

    public void SubmitBuyItem(ShopItemType shopItemType)
    {
        Debug.Log(shopItemType);
        Debug.Log(itemChoose);
        // Check user money
        if (UserLoaderManager.userInGame.gold < int.Parse(textTotal.text))
        {
            soundButtonManager.PlaySFX(soundButtonManager.failed);
            NotificationManager.instance.ShowNotification("You don't have engough money to buy item", 3);
        }
        else
        {
            if (UserLoaderManager.userInGame.gold < int.Parse(textTotal.text))
            {
                soundButtonManager.PlaySFX(soundButtonManager.failed);
                NotificationManager.instance.ShowNotification("You don't have engough money to buy item", 3);
            }
            else
            {
                soundButtonManager.PlaySFX(soundButtonManager.success);
                switch (shopItemType)
                {
                    case ShopItemType.Seeds:
                        Debug.Log($"Add {itemChoose} : {currentQuantity}");
                        UserLoaderManager.userInGame.AddItemToBagAndLoadUI(itemChoose, currentQuantity);
                        UserLoaderManager.userInGame.gold -= int.Parse(textTotal.text);
                        firebaseWriteData.WriteData("Users/" + UserLoaderManager.userInGame.id, UserLoaderManager.userInGame.ToString());
                        break;

                    case ShopItemType.Animals:
                        switch (itemChoose)
                        {
                            case ItemType.Chicken:
                                for (int i = 0; i < currentQuantity; i++)
                                {
                                    animalManager.InstanceNewChicken();
                                }
                                UserLoaderManager.userInGame.gold -= int.Parse(textTotal.text);
                                firebaseWriteData.WriteData("Users/" + UserLoaderManager.userInGame.id, UserLoaderManager.userInGame.ToString());
                                break;
                            case ItemType.Pig:
                                for (int i = 0; i < currentQuantity; i++)
                                {
                                    animalManager.InstanceNewPiggy();
                                }
                                UserLoaderManager.userInGame.gold -= int.Parse(textTotal.text);
                                firebaseWriteData.WriteData("Users/" + UserLoaderManager.userInGame.id, UserLoaderManager.userInGame.ToString());
                                break;
                            case ItemType.Cow:
                                for (int i = 0; i < currentQuantity; i++)
                                {
                                    animalManager.InstanceNewCow();
                                }
                                UserLoaderManager.userInGame.gold -= int.Parse(textTotal.text);
                                firebaseWriteData.WriteData("Users/" + UserLoaderManager.userInGame.id, UserLoaderManager.userInGame.ToString());
                                break;
                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }
                gameObject.SetActive(false);
            }
        }
    }
}
