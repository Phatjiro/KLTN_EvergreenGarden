using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CartManager : MonoBehaviour
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
                SetUpItemCart(spriteChicken, currentQuantity, 50);
                break;

            case ItemType.Pig:
                SetUpItemCart(spritePig, currentQuantity, 100);
                break;

            case ItemType.Cow:
                SetUpItemCart(spriteCow, currentQuantity, 150);
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
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
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
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        currentQuantity += 1;
        SetUpSellBoard();
    }

    public void SubmitBuyItem(ShopItemType shopItemType)
    {
        Debug.Log(shopItemType);
        Debug.Log(itemChoose);
        // Check user money
        if (userLoaderManager.userInGame.gold < int.Parse(textTotal.text))
        {
            NotificationManager.instance.ShowNotification("You don't have engough money to buy item", 3);
        }
        else
        {
            if (userLoaderManager.userInGame.gold < int.Parse(textTotal.text))
            {
                NotificationManager.instance.ShowNotification("You don't have engough money to buy item", 3);
            }
            else
            {
                switch (shopItemType)
                {
                    case ShopItemType.Seeds:
                        Debug.Log($"Add {itemChoose} : {currentQuantity}");
                        userLoaderManager.userInGame.AddItemToBagAndLoadUI(itemChoose, currentQuantity);
                        userLoaderManager.userInGame.gold -= int.Parse(textTotal.text);
                        firebaseWriteData.WriteData("Users/" + userLoaderManager.userInGame.id, userLoaderManager.userInGame.ToString());
                        break;

                    case ShopItemType.Animals:
                        switch (itemChoose)
                        {
                            case ItemType.Chicken:
                                for (int i = 0; i < currentQuantity; i++)
                                {
                                    animalManager.InstanceNewChicken();
                                }
                                userLoaderManager.userInGame.gold -= int.Parse(textTotal.text);
                                firebaseWriteData.WriteData("Users/" + userLoaderManager.userInGame.id, userLoaderManager.userInGame.ToString());
                                break;
                            case ItemType.Pig:
                                for (int i = 0; i < currentQuantity; i++)
                                {
                                    animalManager.InstanceNewPiggy();
                                }
                                userLoaderManager.userInGame.gold -= int.Parse(textTotal.text);
                                firebaseWriteData.WriteData("Users/" + userLoaderManager.userInGame.id, userLoaderManager.userInGame.ToString());
                                break;
                            case ItemType.Cow:
                                for (int i = 0; i < currentQuantity; i++)
                                {
                                    animalManager.InstanceNewCow();
                                }
                                userLoaderManager.userInGame.gold -= int.Parse(textTotal.text);
                                firebaseWriteData.WriteData("Users/" + userLoaderManager.userInGame.id, userLoaderManager.userInGame.ToString());
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
