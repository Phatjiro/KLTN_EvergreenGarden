using UnityEngine;
using UnityEngine.UI;

public class AnimalShopManager : MonoBehaviour
{
    [SerializeField]
    Button buttonChicken;
    [SerializeField]
    Button buttonPig;
    [SerializeField]
    Button buttonCow;
    [SerializeField]
    Button buttonExit;

    [SerializeField]
    GameObject cartObject;

    SoundButtonManager soundButtonManager;

    // Start is called before the first frame update
    void Start()
    {
        soundButtonManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundButtonManager>();

        buttonChicken.onClick.AddListener(() =>
        {
            cartObject.GetComponent<CartManager>().OpenCart(ItemType.Chicken);
        });

        buttonPig.onClick.AddListener(() =>
        {
            cartObject.GetComponent<CartManager>().OpenCart(ItemType.Pig);
        });

        buttonCow.onClick.AddListener(() =>
        {
            cartObject.GetComponent<CartManager>().OpenCart(ItemType.Cow);
        });

        buttonExit.onClick.AddListener(ExitSeedShop);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExitSeedShop()
    {
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        gameObject.SetActive(false);
        if (cartObject.activeSelf == true)
        {
            cartObject.SetActive(false);
        }
    }
}
