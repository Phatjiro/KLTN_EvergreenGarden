using Newtonsoft.Json;
using TMPro;

public enum ItemType
{
    Carrot,
    Corn,
    Pig,
    Cow
}
public class ItemInformation
{
    public string name;
    public int buyPrice;
    public int sellPrice;

    public ItemInformation() 
    {
        name = "Default item";
        buyPrice = 1;
        sellPrice = 1;
    }

    public ItemInformation(string name, int buyPrice, int sellPrice)
    {
        this.name = name;
        this.buyPrice = buyPrice;
        this.sellPrice = sellPrice;
    }

    public override string ToString() 
    {
        return JsonConvert.SerializeObject(this);
    }
}

public class ItemInBag
{
    public ItemType type;
    public int quantity;

    public ItemInBag(ItemType type, int quantity)
    {
        this.type = type;
        this.quantity = quantity;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}