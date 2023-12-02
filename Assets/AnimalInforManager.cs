using Newtonsoft.Json;
using System;
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
    Sprite cornImage;
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

    SoundButtonManager soundButtonManager;

    private void Awake()
    {
        btnExit.onClick.AddListener(disableUI);
        btnSell.onClick.AddListener(SellAnimal);
        btnFeed.onClick.AddListener(FeedAnimal);
        btnSkip.onClick.AddListener(SkipAnimal);
    }

    private void SkipAnimal()
    {
        if (UserLoaderManager.userInGame.diamond <= 0)
        {
            soundButtonManager.PlaySFX(soundButtonManager.failed);
            NotificationManager.instance.ShowNotification("You don't have enoungh skiping fee", 3);
        } 
        else
        {
            soundButtonManager.PlaySFX(soundButtonManager.success);
            UserLoaderManager.userInGame.diamond -= 1;
            firebaseWriteData.WriteData("Users/" + UserLoaderManager.userInGame.id, UserLoaderManager.userInGame.ToString());
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
                    if (UserLoaderManager.userInGame.userBag.GetQuantityOfType(ItemType.Rice) >= 10)
                    {
                        UserLoaderManager.userInGame.SellItemAndLoadUI(ItemType.Rice, 10);
                        firebaseWriteData.WriteData("Users/" + UserLoaderManager.userInGame.id, UserLoaderManager.userInGame.ToString());
                        crrAnimal.timeGrowsUp /= 2;
                        btnFeed.interactable = false;
                        soundButtonManager.PlaySFX(soundButtonManager.success);
                    }
                    else 
                    {
                        NotificationManager.instance.ShowNotification("You don't have enoungh feeding fee",3);
                        soundButtonManager.PlaySFX(soundButtonManager.failed);
                    }
                    break;
                }
            case "Piggy":
                {
                    if (UserLoaderManager.userInGame.userBag.GetQuantityOfType(ItemType.Carrot) >= 10)
                    {
                        UserLoaderManager.userInGame.SellItemAndLoadUI(ItemType.Carrot, 10);
                        firebaseWriteData.WriteData("Users/" + UserLoaderManager.userInGame.id, UserLoaderManager.userInGame.ToString());
                        crrAnimal.timeGrowsUp /= 2;
                        btnFeed.interactable = false;
                        soundButtonManager.PlaySFX(soundButtonManager.success);
                    }
                    else
                    {
                        NotificationManager.instance.ShowNotification("You don't have enoungh feeding fee", 3);
                        soundButtonManager.PlaySFX(soundButtonManager.failed);
                    }

                    break;
                }
            case "Cow":
                {
                    if (UserLoaderManager.userInGame.userBag.GetQuantityOfType(ItemType.Corn) >= 10)
                    {
                        UserLoaderManager.userInGame.SellItemAndLoadUI(ItemType.Corn, 10);
                        firebaseWriteData.WriteData("Users/" + UserLoaderManager.userInGame.id, UserLoaderManager.userInGame.ToString());
                        crrAnimal.timeGrowsUp /= 2;
                        btnFeed.interactable = false;
                        soundButtonManager.PlaySFX(soundButtonManager.success);
                    }
                    else
                    {
                        NotificationManager.instance.ShowNotification("You don't have enoungh feeding fee", 3);
                        soundButtonManager.PlaySFX(soundButtonManager.failed);
                    }

                    break;
                }
            default:
                break;
        }
    }

    public void SellAnimal()
    {
        soundButtonManager.PlaySFX(soundButtonManager.success);

        UserLoaderManager.userInGame.gold += crrAnimal.sellPrice;
        
        bredAnimalDBManager.RemoveAnimal(crrAnimal);
        
        firebaseWriteData.WriteData("Users/" + UserLoaderManager.userInGame.id, UserLoaderManager.userInGame.ToString());
        
        firebaseWriteData.WriteData("Animals/" + UserLoaderManager.userInGame.id, JsonConvert.SerializeObject(bredAnimalDBManager.lstCurrentBredAnimal));
      
        Destroy(crrAnimalGO);

        disableUI();
    }

    private void disableUI()
    {
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        soundButtonManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundButtonManager>();
        btnSell.interactable = false;
    }

    private void OnDisable()
    {
        isLoading = false;
        btnSell.interactable = false;
        btnFeed.interactable = true;
        btnSkip.interactable = true;

        CharacterActionController.isAllowToMove = true;
    }

    private void OnEnable()
    {
        CharacterActionController.isAllowToMove = false;
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

        Debug.Log("Load animal information: " + animal.name);
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
                    imageItemFeeding.sprite = cornImage;
                    if (animal.timeGrowsUp == 50) btnFeed.interactable = false;
                    break;
                }
            default:
                break;
        }

    }
}
