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
    Button buttonExit;

    // Cart
    [SerializeField]
    GameObject cartObject;
    [SerializeField]
    Image imageItem;

    [SerializeField]
    Text textQuantity;
    private int currtentQuantity = 1;

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

    // Sprite
    [SerializeField]
    Sprite spriteCarrot;
    [SerializeField]
    Sprite spriteCorn;

    ItemType itemChoose;

    [SerializeField]
    UserLoaderManager userLoaderManager;
    [SerializeField]
    FirebaseWriteData firebaseWriteData;

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

        buttonExit.onClick.AddListener(ExitSeedShop);
        buttonPlus.onClick.AddListener(PlusItem);
        buttonMinus.onClick.AddListener(MinusItem);
        buttonSubmit.onClick.AddListener(SubmitBuyItem);
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
                SetUpItemCart(spriteCarrot, currtentQuantity, 2);
                break;

            case ItemType.Corn:
                SetUpItemCart(spriteCorn, currtentQuantity, 4);
                break;

            default:
                break;
        }
    }

    public void OpenCart(ItemType item)
    {
        itemChoose = item;
        currtentQuantity = 1;
        cartObject.SetActive(true);
        SetUpCart();
    }

    public void PlusItem()
    {
        currtentQuantity += 1;
        SetUpCart();
    }

    public void MinusItem()
    {
        if (currtentQuantity > 1)
        {
            currtentQuantity -= 1;
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

    public void SubmitBuyItem()
    {
        userLoaderManager.userInGame.AddItemToBag(new ItemInBag(itemChoose, 1), currtentQuantity);
        if (userLoaderManager.userInGame.gold < int.Parse(textTotal.text))
        {
            NotificationManager.instance.ShowNotification("You don't have engough money to buy item", 3);
        }
        else
        {
            userLoaderManager.userInGame.gold -= int.Parse(textTotal.text);
            firebaseWriteData.WriteData("Users/" + userLoaderManager.userInGame.id, userLoaderManager.userInGame.ToString());
            cartObject.SetActive(false);
        }
    }
}
