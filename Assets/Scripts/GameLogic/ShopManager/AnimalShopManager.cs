using UnityEngine;
using UnityEngine.UI;

public class AnimalShopManager : MonoBehaviour
{
    [SerializeField]
    Button buttonChicken;
    [SerializeField]
    Button buttonPig;
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

    // Sprite
    [SerializeField]
    Sprite spriteChicken;
    [SerializeField]
    Sprite spritePig;

    ItemType itemChoose;

    // Start is called before the first frame update
    void Start()
    {
        buttonChicken.onClick.AddListener(() =>
        {
            OpenCart(ItemType.Chicken);
        });

        buttonPig.onClick.AddListener(() =>
        {
            OpenCart(ItemType.Pig);
        });

        buttonExit.onClick.AddListener(ExitSeedShop);
        buttonPlus.onClick.AddListener(PlusItem);
        buttonMinus.onClick.AddListener(MinusItem);
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
            case ItemType.Chicken:
                SetUpItemCart(spriteChicken, currtentQuantity, 20);
                break;

            case ItemType.Pig:
                SetUpItemCart(spritePig, currtentQuantity, 40);
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
}
