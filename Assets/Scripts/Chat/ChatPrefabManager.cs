using PolyAndCode.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ChatType
{
    World,
    Inbox
}

public class ChatItemInfo
{
    public ChatType chatType;
    public string username;
    public string contain;
}
public class ChatPrefabManager : MonoBehaviour, ICell
{
    [SerializeField]
    Image imageHeader;

    [SerializeField]
    Text textContain;

    int _cellIndex;
    ChatItemInfo _itemInfo;

    

    public void SetText(string username, string contain, ChatType chatType)
    {
        string mess = $"<color=blue>[{username}]</color> {contain}";
        textContain.text = mess;
    }

    public void ConfigureCell(ChatItemInfo item, int index)
    {
        _cellIndex = index;
        _itemInfo = item;

        SetText(item.username, item.contain, item.chatType);
    }
}
