using UnityEngine;
using UnityEngine.UI;

public enum ShopItemType
{ 
    Seeds,
    Animals,
    Tool
}

public class ShopTypeManager : MonoBehaviour
{
    public static ShopItemType crrItemType;
    [SerializeField]
    GameObject shopTypeObject;
    [SerializeField]
    Button buttonSeedShop;
    [SerializeField]
    Button buttonAnimalShop;

    [SerializeField]
    GameObject seedShopObject;
    [SerializeField]
    GameObject animalShopObject;

    private void Awake()
    {
        buttonSeedShop.onClick.AddListener(LoadToSeedShop);
        buttonAnimalShop.onClick.AddListener(LoadToAnimalShop);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadToSeedShop()
    {
        crrItemType = ShopItemType.Seeds;
        shopTypeObject.SetActive(false);
        seedShopObject.SetActive(true);
    }

    public void LoadToAnimalShop()
    {
        crrItemType = ShopItemType.Animals;
        shopTypeObject.SetActive(false);
        animalShopObject.SetActive(true);
    }
}
