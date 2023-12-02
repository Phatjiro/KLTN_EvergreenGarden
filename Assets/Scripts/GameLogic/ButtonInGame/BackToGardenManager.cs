using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToGardenManager : MonoBehaviour, ReadDataCallback
{
    bool isLoadedMap = false;
    bool isLoadedAnimal = false;


    [SerializeField]
    Button buttonBackToGarden;
    [SerializeField]
    FirebaseReadData firebaseReadData;
    [SerializeField]
    MapLoaderManager mapLoaderManager;
    [SerializeField]
    AnimalManager animalManager;

    private void Awake()
    {
        buttonBackToGarden.onClick.AddListener(BackToGarden);
    }

    // Start is called before the first frame update
    void Start()
    {
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
    public void ReloadScene()
    {
        Debug.Log("Reload Scene");
        Scene scene = SceneManager.GetActiveScene();
        LoadingManager.NEXT_SCENE = scene.name;
        SceneManager.LoadScene("LoadingScene");
    }

    public void ShowButtonBack()
    { 
        buttonBackToGarden.gameObject.SetActive(true);
    }

    public void HideButtonBack() 
    {
        buttonBackToGarden.gameObject.SetActive(false);
    }

    public void BackToGarden()
    {
        FriendItemCanvasManager.isThisUser = true;
        MapLoaderManager.isLoadedMap = false;
        BredAnimalDBManager.isLoadedAnimal = false;
        ReloadScene();
        /*firebaseReadData.ReadData("Maps/" + UserLoaderManager.userInGame.id, this, ReadDataType.Map);
        firebaseReadData.ReadData("Animals/" + UserLoaderManager.userInGame.id, this, ReadDataType.Animal);*/
    }

    public void OnReadDataMapCompleted(string data)
    {
        FriendItemCanvasManager.isNeedToLoadVisitMap = true;
        FriendItemCanvasManager.MapJsonWaitToLoad = data;
        isLoadedMap = true;
        this.enabled = true;
        //mapLoaderManager.LoadMap(JsonConvert.DeserializeObject<Map>(data));
    }

    public void OnReadDataUserCompleted(string data)
    {
        return;
    }

    public void OnReadDataAnimalCompleted(string data)
    {
        FriendItemCanvasManager.isNeedToLoadVisitMap = true;
        FriendItemCanvasManager.AnimalJsonWaitToLoad = data;
        isLoadedAnimal = true;
        //animalManager.LoadAnimal(JsonConvert.DeserializeObject<List<Animal>>(data));
    }

    public void OnReadDataAllUserCompleted(List<string> data)
    {
        return;
    }
}
