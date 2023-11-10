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

    public User()
    {
        id = "abcxyz";
        characterName = "Default name";
        gold = 0;
        diamond = 0;

        userBag = new Bag();
    }

    public User(string id, string characterName, int gold, int diamond)
    {
        this.id = id;
        this.characterName = characterName;
        this.gold = gold;
        this.diamond = diamond;
    }

    public void ShowBag()
    {
        foreach (var item in userBag.lstItem)
        {
            Debug.Log(item.ToString());
        }
    }

    public void UpdateBagUI(ItemType itemType, int quantity)
    {
        Sprite iconSprite = ItemInformationManager._instance.GetIcon(itemType);
        if (BagUIManager._instance != null)
            BagUIManager._instance.UpdateItemAt(0, iconSprite, quantity);
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
