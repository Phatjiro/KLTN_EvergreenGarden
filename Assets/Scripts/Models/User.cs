using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class User
{
    public string id;
    public string characterName;
    public int gold;
    public int diamond;

    public Bag userBag;
    public Map userMap;

    public User()
    {
        id = "abcxyz";
        characterName = "Default name";
        gold = 0;
        diamond = 0;

        userBag = new Bag();
        userMap = new Map();
    }

    public User(string id, string characterName, int gold, int diamond)
    {
        this.id = id;
        this.characterName = characterName;
        this.gold = gold;
        this.diamond = diamond;
    }

    public User(string id, string characterName, int gold, int diamond, Bag userBag, Map userMap)
    {
        this.id = id;
        this.characterName = characterName;
        this.gold = gold;
        this.diamond = diamond;
        this.userBag = userBag;
        this.userMap = userMap;
    }

    public void ShowBag()
    {
        foreach (var item in userBag.lstItem)
        {
            Debug.Log(item.ToString());
        }
    }

    private void AddItemToBagListToShow(ItemType itemType, int quantity)
    {
        BagItemLoader loader = BagItemLoader.instance;
        if (loader != null)
        {
            loader.AddItemToLst(new ItemInBag(itemType, quantity));
        }
    }

    public void AddItemToBag(ItemInBag itemInBag, int quantity)
    {
        if (this.userBag == null)
            this.userBag = new Bag();
        this.userBag.AddItem(itemInBag, quantity);
        AddItemToBagListToShow(itemInBag.type, quantity);
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
