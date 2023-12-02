using DG.Tweening;
using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FriendListManager : MonoBehaviour, ReadDataCallback
{
    public static FriendListManager _instance;

    [SerializeField]
    Button buttonFiendList;
    [SerializeField]
    GameObject friendListObject;
    [SerializeField]
    Button buttonExit;

    public FirebaseUser firebaseUser;
    [SerializeField]
    FirebaseReadData firebaseReadData;
    public List<User> lstAllUser;

    Boolean isLoadFirstTime = true;

    private static bool isShowing = false;

    SoundButtonManager soundButtonManager;

    private void Awake()
    {
        firebaseUser = FirebaseAuth.DefaultInstance.CurrentUser;

        buttonFiendList.onClick.AddListener(TurnOnOffFriendList);
        buttonExit.onClick.AddListener(ExitFriendList);
    }

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        soundButtonManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundButtonManager>();
        if (firebaseUser != null)
        {
            firebaseReadData.ReadData("Users", this, ReadDataType.AllUser);
        }
    }

    public static bool IsShowing()
    {
        return isShowing;
    }

    public void TurnOnOffFriendList()
    {
        if (isShowing)
            ExitFriendList();
        else
            ShowFriendList();
    }

    public void ShowFriendList()
    {
        isShowing = true;
        this.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f);

        CharacterActionController.isAllowToMove = false;
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
    }


    public void ExitFriendList()
    {
        isShowing = false;
        this.GetComponent<RectTransform>().DOAnchorPosY(-1000, 0.5f);
        CharacterActionController.isAllowToMove = true;
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
    }

    public void OnReadDataMapCompleted(string data)
    {
        return;
    }

    public void OnReadDataUserCompleted(string data)
    {
        
    }

    public void OnReadDataAnimalCompleted(string data)
    {
        return;
    }

    public void OnReadDataAllUserCompleted(List<string> data)
    {
#if !UNITY_EDITOR
        lstAllUser = new List<User>();
        for (int i = 0; i < data.Count; i++)
        {
            lstAllUser.Add(JsonConvert.DeserializeObject<User>(data[i]));
        }

        lstAllUser = lstAllUser.OrderByDescending(user => user.gold).ToList();

        if (isLoadFirstTime)
        {
            isLoadFirstTime = false;
            FriendItemLoader loader = FriendItemLoader.Instance;
            if (loader != null)
            {
                Debug.Log("Vao load UI Friend");
                loader.SetLstItem(lstAllUser);
                loader.ReloadUI();
            }
        }
#endif
    }
}
