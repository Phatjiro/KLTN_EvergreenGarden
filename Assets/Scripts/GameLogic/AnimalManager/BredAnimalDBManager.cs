using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BredAnimalDBManager : MonoBehaviour, ReadDataCallback
{
    public static bool isLoadedAnimal = false;

    public List<Animal> lstCurrentBredAnimal = new List<Animal>();

    public BredAnimalDBManager() { }
    [SerializeField]
    private FirebaseWriteData writeData;
    [SerializeField]
    AnimalManager animalManager;

    private FirebaseUser firebaseUser;
    [SerializeField]
    private FirebaseReadData readData;

    private void Awake()
    {
        loadDataDB();

        if (FriendItemCanvasManager.isNeedToLoadVisitMap)
        {
            LoadAnimalByJson(FriendItemCanvasManager.AnimalJsonWaitToLoad);
        }
    }
    private void Start()
    {
        Debug.Log("BredAnimalDBManager" + gameObject);
    }
    public BredAnimalDBManager(List<Animal> lstCurrentBredAnimal)
    {
        this.lstCurrentBredAnimal = lstCurrentBredAnimal;
    }
    public void addAnimal(Animal item)
    {
        if (lstCurrentBredAnimal == null)
        {
            lstCurrentBredAnimal = new List<Animal>();
        }
        lstCurrentBredAnimal.Add(item);     
       
    }

    public int getLength()
    {
        return lstCurrentBredAnimal.Count;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
    public void addToDB()
    {
#if UNITY_EDITOR
        return;
#endif

        firebaseUser = FirebaseAuth.DefaultInstance.CurrentUser;
        Debug.Log("start write data animal" + JsonConvert.SerializeObject(lstCurrentBredAnimal));
        writeData.WriteData("Animals/" + firebaseUser.UserId, JsonConvert.SerializeObject(lstCurrentBredAnimal));
    }

    public void loadDataDB()
    {
#if UNITY_EDITOR
        return;
#endif
        firebaseUser = FirebaseAuth.DefaultInstance.CurrentUser;

        Debug.Log("start read data animal");
        Debug.Log("firebase user null: " + firebaseUser == null);
        readData.ReadData("Animals/" + firebaseUser.UserId, this, ReadDataType.Animal);
        
    }

    public void OnReadDataMapCompleted(string data)
    {
        
    }

    public void OnReadDataUserCompleted(string data)
    {
       
    }

    public void OnReadDataAnimalCompleted(string data)
    {
        if (isLoadedAnimal) return;
        isLoadedAnimal = true;
        LoadAnimalByJson(data);
    }

    public void LoadAnimalByJson(string json)
    {
        Debug.Log("start Read");
        lstCurrentBredAnimal = JsonConvert.DeserializeObject<List<Animal>>(json);
        Debug.Log("Convert Json to List Animal" + json);
        animalManager.LoadAnimal(lstCurrentBredAnimal);
    }

    public void RemoveAnimal(Animal rmAnimal)
    {
        Debug.Log("start remove");

        lstCurrentBredAnimal.Remove(rmAnimal);

        Debug.Log("remove sucesses");
    }

    public void OnReadDataAllUserCompleted(List<string> data)
    {
        
    }
}