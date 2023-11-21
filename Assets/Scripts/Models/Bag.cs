using Newtonsoft.Json;
using System.Collections.Generic;

public class Bag
{
    public List<ItemInBag> lstItem { get; set; }

    public Bag() 
    {
        lstItem = new List<ItemInBag>();
    }

    public Bag(List<ItemInBag> lstItem)
    {
        this.lstItem = lstItem;
    }

    public void AddItem(ItemType itemType, int quantity) 
    {
        if (lstItem == null) lstItem = new List<ItemInBag>();
        for (int i = 0; i < lstItem.Count; i++)
        {
            if (lstItem[i].type == itemType)
            {
                lstItem[i].quantity += quantity;
                return;
            }
        }
        lstItem.Add(new ItemInBag(itemType, quantity));
        return;
    }
    public void RemoveItem(ItemType itemType, int quantity)
    {
        if (lstItem == null) return;
        for (int i = 0; i < lstItem.Count; i++)
        {
            if (lstItem[i].type == itemType)
            {
                int crrQuantity = lstItem[i].quantity;
                crrQuantity -= quantity;
                if (crrQuantity < 0)
                    crrQuantity = 0;
                lstItem[i].quantity = crrQuantity;
                return;
            }
        }
    }
    public int GetQuantityOfType(ItemType itemType)
    {
        foreach (ItemInBag item in lstItem)
        {
            if (itemType == item.type)
                return item.quantity;
        }
        return 0;
    }

    public int GetLength()
    { 
        return lstItem.Count;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this); 
    }
}
