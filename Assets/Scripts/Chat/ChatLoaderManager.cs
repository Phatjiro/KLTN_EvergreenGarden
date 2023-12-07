using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using PolyAndCode.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatLoaderManager : MonoBehaviour, IRecyclableScrollRectDataSource, ReadDataCallback
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    [SerializeField]
    private int _dataLength;

    [SerializeField]
    InputField fieldChat;
    [SerializeField]
    Button buttonSubmit;

    [SerializeField]
    FirebaseWriteData firebaseWriteData;
    [SerializeField]
    FirebaseReadData firebaseReadData;

    private List<ChatItemInfo> lstItem = new List<ChatItemInfo>();
    public int GetItemCount()
    {
        return lstItem.Count;
    }


    private void Awake()
    {
        _recyclableScrollRect.DataSource = this;
        _recyclableScrollRect.ReloadData();

        buttonSubmit.onClick.AddListener(SendMessage);
    }

    private void Start()
    {
        firebaseReadData.ReadData("Chats/WorldChat", this, ReadDataType.Chat);
        FirebaseDatabase.DefaultInstance.GetReference("Chats/WorldChat").ValueChanged += OnDataChanged;

        //List<ChatItemInfo> lstDemo = new List<ChatItemInfo> ();
        //for (int i = 0; i< 50; i++)
        //{
        //    ChatItemInfo item = new ChatItemInfo();
        //    item.username = "User " + i;
        //    item.contain = " day la user " + i;
        //    item.chatType = ChatType.World;

        //    lstDemo.Add(item);
        //}

        //this.lstItem = lstDemo;
        //_recyclableScrollRect.ReloadData();
    }

    public void SetCell(ICell cell, int index)
    {
        //Casting to the implemented Cell
        var item = cell as ChatPrefabManager;
        item.ConfigureCell(this.lstItem[index], index);
    }

    public void SendMessage()
    {
        ChatItemInfo chatItemInfo = new ChatItemInfo();
        chatItemInfo.chatType = ChatType.World;
        chatItemInfo.username = UserLoaderManager.userInGame.characterName;
        chatItemInfo.contain = fieldChat.text;
        lstItem.Add(chatItemInfo);
        firebaseWriteData.WriteData("Chats/WorldChat", JsonConvert.SerializeObject(lstItem));
    }

    public void OnReadDataMapCompleted(string data)
    {
        return;
    }

    public void OnReadDataUserCompleted(string data)
    {
        return;
    }

    public void OnReadDataAnimalCompleted(string data)
    {
        return;
    }

    public void OnReadDataAllUserCompleted(List<string> data)
    {
        return;
    }

    public void OnReadDataChatCompleted(string data)
    {
        List<ChatItemInfo> lstChatInfo = JsonConvert.DeserializeObject<List<ChatItemInfo>>(data);
        this.lstItem = lstChatInfo;
        _recyclableScrollRect.ReloadData();
    }

    public void OnDataChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        firebaseReadData.ReadData("Chats/WorldChat", this, ReadDataType.Chat);
    }
}
