using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class User
{
    public string id { get; set; }
    public string characterName { get; set; }
    public int gold { get; set; }
    public int diamond { get; set; }

    public Bag userBag { get; set; }


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
    
    public User(string id, string characterName, int gold, int diamond, Bag userBag)
    {
        this.id = id;
        this.characterName = characterName;
        this.gold = gold;
        this.diamond = diamond;
        this.userBag = userBag;
	}

    public User(string id, string characterName, int gold, int diamond, Bag userBag)
    {
        this.id = id;
        this.characterName = characterName;
        this.gold = gold;
        this.diamond = diamond;
        this.userBag = userBag;
    }

    public void ShowBag()
    {
        foreach (var item in userBag.lstItem)
        {
            Debug.Log(item.ToString());
        }
    }

    private void ReloadBagUI(ItemType itemType, int quantity)
    {
        BagItemLoader loader = BagItemLoader.instance;
        if (loader != null)
        {
            loader.SetLstItem(this.userBag.lstItem);
            loader.ReloadUI();
        }
    }

    public void AddItemToBagAndLoadUI(ItemType itemType, int quantity)
    {
        if (this.userBag == null)
            this.userBag = new Bag();
        this.userBag.AddItem(itemType, quantity);
        ReloadBagUI(itemType, quantity);
    }
    public void SellItemAndLoadUI(ItemType itemType, int quantity)
    {
        Debug.Log("Sell Item and load UI");
        if (this.userBag == null)
            this.userBag = new Bag();
        this.userBag.RemoveItem(itemType, quantity);
        ReloadBagUI(itemType, quantity);
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
