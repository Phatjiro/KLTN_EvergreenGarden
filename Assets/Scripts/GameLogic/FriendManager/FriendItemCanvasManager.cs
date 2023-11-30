using PolyAndCode.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendItemCanvasManager : MonoBehaviour, ICell
{
    [SerializeField]
    Text textCharacterName;
    [SerializeField]
    Text textUID;

    //Model
    private User _itemInfo;
    private int _cellIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCharacterName(string name)
    { 
        this.textCharacterName.text = "NAME: " + name;
    }

    public void SetUID(string uid) 
    {
        this.textUID.text = "UID: " + uid;
    }

    public void ConfigureCell(User item, int index)
    {
        _cellIndex = index;
        _itemInfo = item;

        SetCharacterName(item.characterName);
        SetUID(item.id);
    }
}
