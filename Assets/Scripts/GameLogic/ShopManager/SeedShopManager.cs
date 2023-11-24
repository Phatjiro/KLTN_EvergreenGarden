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

    [SerializeField]
    GameObject cartObject;

    SoundButtonManager soundButtonManager;

    // Start is called before the first frame update
    void Start()
    {
        soundButtonManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundButtonManager>();

        buttonCarrot.onClick.AddListener(() =>
        {
            cartObject.GetComponent<CartManager>().OpenCart(ItemType.Carrot);
        });

        buttonCorn.onClick.AddListener(() =>
        {
            cartObject.GetComponent<CartManager>().OpenCart(ItemType.Corn);
        });

        buttonRice.onClick.AddListener(() =>
        {
            cartObject.GetComponent<CartManager>().OpenCart(ItemType.Rice);
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
