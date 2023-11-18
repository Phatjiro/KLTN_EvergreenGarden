using UnityEngine;
using UnityEngine.UI;

public class ShopTypeManager : MonoBehaviour
{
    [SerializeField]
    GameObject shopTypeObject;
    [SerializeField]
    Button buttonSeedShop;
    [SerializeField]
    Button buttonAnimalShop;

    [SerializeField]
    GameObject seedShopObject;

    private void Awake()
    {
        buttonSeedShop.onClick.AddListener(LoadToSeedShop);
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
        shopTypeObject.SetActive(false);
        seedShopObject.SetActive(true);
    }
}
