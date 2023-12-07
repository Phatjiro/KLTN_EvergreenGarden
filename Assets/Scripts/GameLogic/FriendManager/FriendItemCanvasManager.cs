using Firebase.Auth;
using Newtonsoft.Json;
using PolyAndCode.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FriendItemCanvasManager : MonoBehaviour, ICell, ReadDataCallback
{
    public static string MapJsonWaitToLoad = "";
    public static string AnimalJsonWaitToLoad = "";
    public static bool isNeedToLoadVisitMap = false;
    public static bool isThisUser = true;

    bool isLoadedMap = false;
    bool isLoadedAnimal = false;

    [SerializeField]
    Image imageBackground;
    [SerializeField]
    Text textRank;
    [SerializeField]
    Text textCharacterName;
    [SerializeField]
    Text textUID;
    [SerializeField]
    Text textGold;
    [SerializeField]
    Text textDiamond;

    [SerializeField]
    Button buttonGift;
    [SerializeField]
    Button buttonVisit;

    FirebaseReadData firebaseReadData;
    MapLoaderManager mapLoaderManager;
    AnimalManager animalManager;
    BackToGardenManager backToGardenManager;

    //Model
    private User _itemInfo;
    private int _cellIndex;

    private void Awake()
    {
        buttonVisit.onClick.AddListener(VisitFriendGarden);
    }

    // Start is called before the first frame update
    void Start()
    {
        firebaseReadData = GameObject.FindGameObjectWithTag("Firebase").GetComponent<FirebaseReadData>();
        mapLoaderManager = GameObject.FindGameObjectWithTag("LoadMap").GetComponent<MapLoaderManager>();
        animalManager = GameObject.FindGameObjectWithTag("LoadAnimal").GetComponent<AnimalManager>();
        backToGardenManager = GameObject.FindGameObjectWithTag("BackGarden").GetComponent<BackToGardenManager>();
        if (isThisUser)
        {
            backToGardenManager.HideButtonBack();
        }
        else
        {
            backToGardenManager.ShowButtonBack();
        }
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoadedMap && isLoadedAnimal)
        {
            ReloadScene();
        }
    }

    public void SetRank(int index)
    { 
        this.textRank.text = index.ToString();
    }

    public void SetCharacterName(string name)
    {
        if (name == null)
        {
            this.textCharacterName.text = "Undefine";
            return;
        }    
        if (name.Length <= 12)
        {
            this.textCharacterName.text = "Name: " + name;
        }
        else
        {
            this.textCharacterName.text = "Name: " + name.Substring(0, 12);
        }
    }

    public void SetUID(string uid)
    {
        this.textUID.text = "Uid: " + uid.Substring(0, Mathf.Min(12, uid.Length));
    }

    public void SetGold(string gold)
    { 
        this.textGold.text = "Gold: " + gold;
    }

    public void SetDiamond(string diamond)
    { 
        this.textDiamond.text = "Diamond: " + diamond;
    }

    public void SetBackgroundYouInRank(bool isThisUser)
    {
        if (isThisUser)
            this.imageBackground.color = new Color(255f / 255f, 136f / 255f, 64f / 255f);
        else
            this.imageBackground.color = new Color(0, 156f / 255f, 121f / 255f);
    }

    public void ConfigureCell(User item, int index)
    {
        _cellIndex = index;
        _itemInfo = item;

        int i = 1;
        Debug.Log(i++);
        SetRank(index+1);
        Debug.Log(i++);
        SetCharacterName(item.characterName);
        Debug.Log(i++);
        SetUID(item.id);
        Debug.Log(i++);
        SetGold(item.gold.ToString());
        Debug.Log(i++);
        SetDiamond(item.diamond.ToString());
        Debug.Log(i++);
        if (UserLoaderManager.userInGame != null && item.id == UserLoaderManager.userInGame.id)
        {
            SetBackgroundYouInRank(true);
            buttonGift.interactable = false;
            buttonVisit.interactable = false;
        }
        else
        {
            SetBackgroundYouInRank(false);
            buttonGift.interactable = true;
            buttonVisit.interactable = true;
        }
#if UNITY_EDITOR
        if (item.id == "5")
        {
            SetBackgroundYouInRank(true);
        }
        else
        {
            SetBackgroundYouInRank(false);
        }
#endif
    }

    public void VisitFriendGarden()
    {
        isThisUser = false;
        firebaseReadData.ReadData("Maps/" + _itemInfo.id, this, ReadDataType.Map);
        firebaseReadData.ReadData("Animals/" + _itemInfo.id, this, ReadDataType.Animal);
        backToGardenManager.ShowButtonBack();
    }

    public void OnReadDataMapCompleted(string data)
    {
        Debug.Log("OnReadDataMapCompleted - FriendVisit");
        isNeedToLoadVisitMap = true;
        MapJsonWaitToLoad = data;
        isLoadedMap = true;
        this.enabled = true;


    }
    public void LoadMap(string data)
    {
        mapLoaderManager.LoadMap(JsonConvert.DeserializeObject<Map>(data));
    }

    public void ReloadScene()
    {
        Debug.Log("Reload Scene");
        Scene scene = SceneManager.GetActiveScene();
        LoadingManager.NEXT_SCENE = scene.name;
        SceneManager.LoadScene("LoadingScene");
    }

    public void OnReadDataUserCompleted(string data)
    {
        return;
    }

    public void OnReadDataAnimalCompleted(string data)
    {
        AnimalJsonWaitToLoad = data;
        isNeedToLoadVisitMap = true;
        isLoadedAnimal = true;
/*        animalManager.LoadAnimal(JsonConvert.DeserializeObject<List<Animal>>(data));*/
    }

    public void OnReadDataAllUserCompleted(List<string> data)
    {
        return;
    }

    public void OnReadDataChatCompleted(string data)
    {
        return;
    }
}
