using DG.Tweening;
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
    Button buttonMinimize;
    [SerializeField]
    GameObject worldChatChannelObject;
    private bool isMinimize = true;

    [SerializeField]
    InputField fieldChat;
    [SerializeField]
    Button buttonSubmit;

    [SerializeField]
    FirebaseWriteData firebaseWriteData;
    [SerializeField]
    FirebaseReadData firebaseReadData;

    bool isScrolling = false;

    private List<ChatItemInfo> lstItem = new List<ChatItemInfo>();
    public int GetItemCount()
    {
        return lstItem.Count;
    }


    private void Awake()
    {
        _recyclableScrollRect.DataSource = this;
        _recyclableScrollRect.ReloadData();
        _recyclableScrollRect.normalizedPosition = new Vector2(0, 0);

        buttonMinimize.onClick.AddListener(MinimizeChatBox);
        buttonSubmit.onClick.AddListener(SendMessage);
    }

    private void Start()
    {
        firebaseReadData.ReadData("Chats/WorldChat", this, ReadDataType.Chat);
        FirebaseDatabase.DefaultInstance.GetReference("Chats/WorldChat").ValueChanged += OnDataChanged;

        //List<ChatItemInfo> lstDemo = new List<ChatItemInfo>();
        //for (int i = 0; i < 50; i++)
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

        fieldChat.text = "";
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
        StartCoroutine(SrollToBottom());
    }

    public void AddNewChat(string mess)
    {
        ChatItemInfo chatItem = new ChatItemInfo();
        chatItem.username = "test";
        chatItem.contain = mess;

        this.lstItem.Add(chatItem);
        StartCoroutine(SrollToBottom());
    }
    IEnumerator SrollToBottom()
    {
        if (!isScrolling)
        {
            yield return new WaitForSeconds(1);
            int n = lstItem.Count;
            for (int i = 0; i < n; i++)
            {
                isScrolling = true;
                _recyclableScrollRect.verticalNormalizedPosition = 0;
                yield return new WaitForSeconds(0.01f);
            }
            isScrolling = false;
        }
        
    }

    public void MinimizeChatBox()
    {
        if (isMinimize)
        {
            isMinimize = false;
            CharacterActionController.isAllowToMove = false;
            worldChatChannelObject.GetComponent<RectTransform>().DOAnchorPosX(-540, 1);
            worldChatChannelObject.GetComponent<RectTransform>().DOAnchorPosY(800, 1);
        }
        else
        {
            isMinimize = true;
            CharacterActionController.isAllowToMove = true;
            worldChatChannelObject.GetComponent<RectTransform>().DOAnchorPosX(420, 1);
            worldChatChannelObject.GetComponent<RectTransform>().DOAnchorPosY(80, 1);
        }
    }
}
