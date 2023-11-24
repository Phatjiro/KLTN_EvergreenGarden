using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalInforManager : MonoBehaviour
{

    [SerializeField]
    Button btnExit;
    [SerializeField]
    Text txtBuyPrice;
    [SerializeField]
    Text txtSellPrice;
    
    [SerializeField]
    Text txtanimalName;

    [SerializeField] 
    LoadingBarManager loading;

    [SerializeField]
    Image imageItemFeeding;

    [SerializeField]
    Sprite carrotImage;
    [SerializeField]
    Sprite corImage;
    [SerializeField]
    Sprite riceImage;

    Animal crrAnimal;
    GameObject crrAnimalGO;

    [SerializeField]
    Button btnSell;

    [SerializeField]
    Button btnFeed;

    [SerializeField]
    Button btnSkip;

    bool isLoading = false;
    [SerializeField]
    UserLoaderManager userLoaderManager;

    [SerializeField]
    BredAnimalDBManager bredAnimalDBManager;
    [SerializeField]
    FirebaseWriteData firebaseWriteData;

    //private bool isFeeded = false;

    private void Awake()
    {
        btnExit.onClick.AddListener(disableUI);
        btnSell.onClick.AddListener(SellAnimal);
        btnFeed.onClick.AddListener(FeedAnimal);
        btnSkip.onClick.AddListener(SkipAnimal);

    }

    private void SkipAnimal()
    {
        if (userLoaderManager.userInGame.diamond <= 0)
        {
            NotificationManager.instance.ShowNotification("You don't have enoungh skiping fee", 3);
        } else
        {
            userLoaderManager.userInGame.diamond -= 1;
            firebaseWriteData.WriteData("Users/" + userLoaderManager.userInGame.id, userLoaderManager.userInGame.ToString());
            crrAnimal.timeGrowsUp = 1;
            loading.LoadingInTime(2);
            btnSkip.interactable = false;
        }

    }

    private void FeedAnimal()
    {
        switch (crrAnimal.name)
        {
            case "Chicken":
                {
                    if (userLoaderManager.userInGame.userBag.GetQuantityOfType(ItemType.Rice) >= 10)
                    {
                        userLoaderManager.userInGame.SellItemAndLoadUI(ItemType.Rice, 10);
                        firebaseWriteData.WriteData("Users/" + userLoaderManager.userInGame.id, userLoaderManager.userInGame.ToString());
                        crrAnimal.timeGrowsUp /= 2;
                        btnFeed.interactable = false;

                    }
                    else 
                    {
                        NotificationManager.instance.ShowNotification("You don't have enoungh feeding fee",3);

                    }
                    break;
                }
            case "Piggy":
                {
                    if (userLoaderManager.userInGame.userBag.GetQuantityOfType(ItemType.Carrot) >= 10)
                    {
                        userLoaderManager.userInGame.SellItemAndLoadUI(ItemType.Carrot, 10);
                        firebaseWriteData.WriteData("Users/" + userLoaderManager.userInGame.id, userLoaderManager.userInGame.ToString());
                        crrAnimal.timeGrowsUp /= 2;
                        btnFeed.interactable = false;

                    }
                    else
                    {
                        NotificationManager.instance.ShowNotification("You don't have enoungh feeding fee", 3);

                    }

                    break;
                }
            case "Cow":
                {
                    if (userLoaderManager.userInGame.userBag.GetQuantityOfType(ItemType.Corn) >= 10)
                    {
                        userLoaderManager.userInGame.SellItemAndLoadUI(ItemType.Corn, 10);
                        firebaseWriteData.WriteData("Users/" + userLoaderManager.userInGame.id, userLoaderManager.userInGame.ToString());
                        crrAnimal.timeGrowsUp /= 2;
                        btnFeed.interactable = false;

                    }
                    else
                    {
                        NotificationManager.instance.ShowNotification("You don't have enoungh feeding fee", 3);

                    }

                    break;
                }
            default:
                break;
        }
    }

    public void SellAnimal()
    {
        
        userLoaderManager.userInGame.gold += crrAnimal.sellPrice;
        
        bredAnimalDBManager.RemoveAnimal(crrAnimal);
        
        firebaseWriteData.WriteData("Users/" + userLoaderManager.userInGame.id, userLoaderManager.userInGame.ToString());
        
        firebaseWriteData.WriteData("Animals/" + userLoaderManager.userInGame.id, JsonConvert.SerializeObject(bredAnimalDBManager.lstCurrentBredAnimal));
      
        Destroy(crrAnimalGO);

        disableUI();
    }

    private void disableUI()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        btnSell.interactable = false;
    }
    private void OnDisable()
    {
        isLoading = false;
        btnSell.interactable = false;
        btnFeed.interactable = true;
        btnSkip.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLoading) return;
        double crrTimePass = DateTime.Now.Subtract(crrAnimal.buyTime).TotalSeconds;
        float crrPercent = (float)crrTimePass / crrAnimal.timeGrowsUp;
        //Debug.Log("Crr percent: " + crrPercent);
        if (crrPercent >= 1)
        {
            Debug.Log("Enough time");
            btnSell.interactable = true;
            isLoading = false;
            btnFeed.interactable = false;
            btnSkip.interactable= false;
        }
        loading.SetPercent(crrPercent);
    }

    public void loadAnimalInfor(Animal animal, GameObject animaGO)
    {
        this.crrAnimalGO = animaGO;
        this.crrAnimal = animal;
        txtanimalName.text = animal.name;
        txtBuyPrice.text = animal.buyPrice.ToString();
        txtSellPrice.text = animal.sellPrice.ToString();
        double crrTimePass = DateTime.Now.Subtract(animal.buyTime).TotalSeconds;
        float crrPercent = (float)crrTimePass / animal.timeGrowsUp;
        loading.SetPercent(crrPercent);
        isLoading = true;

        switch (animal.name) 
        {
            case "Chicken":
                {
                    imageItemFeeding.sprite = riceImage;
                    if (animal.timeGrowsUp == 15) btnFeed.interactable = false;
                    break;
                }
            case "Piggy":
                {
                    imageItemFeeding.sprite = carrotImage;
                    if (animal.timeGrowsUp == 25) btnFeed.interactable = false;
                    break;
                }
            case "Cow":
                {
                    imageItemFeeding.sprite = corImage;
                    if (animal.timeGrowsUp == 50) btnFeed.interactable = false;
                    break;
                }
            default:
                break;
        }

    }
}
