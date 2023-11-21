using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class SeedShopManager : MonoBehaviour
{
    [SerializeField]
    Button buttonCarrot;
    [SerializeField]
    Button buttonCorn;
    [SerializeField]
    Button buttonRice;
    [SerializeField]
    Button buttonExit;

    // Cart
    [SerializeField]
    GameObject cartObject;
    [SerializeField]
    Image imageItem;

    [SerializeField]
    Text textQuantity;
    private int currentQuantity = 1;

    [SerializeField]
    Text textPrice;
    [SerializeField]
    Text textTotal;

    [SerializeField]
    Button buttonPlus;
    [SerializeField]
    Button buttonMinus;
    [SerializeField]
    Button buttonSubmit;

    ItemType itemChoose;

    [SerializeField]
    UserLoaderManager userLoaderManager;
    [SerializeField]
    FirebaseWriteData firebaseWriteData;

    [SerializeField]
    AnimalManager animalManager;

    // Start is called before the first frame update
    void Start()
    {
        buttonCarrot.onClick.AddListener(() =>
        {
            OpenCart(ItemType.Carrot);
        });

        buttonCorn.onClick.AddListener(() =>
        {
            OpenCart(ItemType.Corn);
        });

        buttonRice.onClick.AddListener(() =>
        {
            OpenCart(ItemType.Rice);
        });

        buttonExit.onClick.AddListener(ExitSeedShop);
        buttonPlus.onClick.AddListener(PlusItem);
        buttonMinus.onClick.AddListener(MinusItem);
        buttonSubmit.onClick.AddListener(()=> {
            SubmitBuyItem(ShopTypeManager.crrItemType);
        });

        gameObject.SetActive(false);
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

    public void SetUpCart()
    {
        switch (itemChoose)
        {
            case ItemType.Carrot:
                SetUpItemCart(ItemInformationManager._instance.GetIcon(ItemType.Carrot), currentQuantity, 2);
                break;

            case ItemType.Corn:
                SetUpItemCart(ItemInformationManager._instance.GetIcon(ItemType.Corn), currentQuantity, 4);
                break;

            case ItemType.Rice:
                SetUpItemCart(ItemInformationManager._instance.GetIcon(ItemType.Rice), currentQuantity, 1);
                break;

            default:
                break;
        }
    }

    public void OpenCart(ItemType item)
    {
        itemChoose = item;
        currentQuantity = 1;
        cartObject.SetActive(true);
        SetUpCart();
    }

    public void PlusItem()
    {
        currentQuantity += 1;
        SetUpCart();
    }

    public void MinusItem()
    {
        if (currentQuantity > 1)
        {
            currentQuantity -= 1;
            SetUpCart();
        }
        else
        {
            NotificationManager.instance.ShowNotification("The quantity of items cannot be less than 1", 4);
            return;
        }
    }

    public void ExitSeedShop()
    { 
        gameObject.SetActive(false);
        if (cartObject.activeSelf == true)
        {
            cartObject.SetActive(false);
        }
    }

    public void SubmitBuyItem(ShopItemType shopItemType)
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
                    switch (AnimalShopManager.itemChoose)
                    {
                        case ItemType.Chicken:
                            for (int i = 0; i < currentQuantity; i++)
                            {
                                animalManager.InstanceNewChicken();
                            }
                            break;
                        case ItemType.Pig:
                            for (int i = 0; i < currentQuantity; i++)
                            {
                                animalManager.InstanceNewPiggy();
                            }
                            break;
                        case ItemType.Cow:
                            for (int i = 0; i < currentQuantity; i++)
                            {
                                animalManager.InstanceNewCow();
                            }                            
                            break;
                        default:
                            break;
                    }
                    userLoaderManager.userInGame.gold -= int.Parse(textTotal.text);
                    break;

                default:
                    break;
            }
            cartObject.SetActive(false);
        }
    }
}
