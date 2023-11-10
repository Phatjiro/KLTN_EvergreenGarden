using Newtonsoft.Json;
using System.Collections.Generic;

public class Bag
{
    public List<ItemInBag> lstItem;

    public Bag() 
    {
        lstItem = new List<ItemInBag>();
    }

    public Bag(List<ItemInBag> lstItem)
    {
        this.lstItem = lstItem;
    }

    public void AddItem(ItemInBag item) 
    {
        if (lstItem == null) lstItem = new List<ItemInBag>();
        for (int i = 0; i < lstItem.Count; i++)
        {
            if (lstItem[i].type == item.type)
            {
                lstItem[i].quantity += 1;
                return;
            }
        }
        lstItem.Add(item);
        return;
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
