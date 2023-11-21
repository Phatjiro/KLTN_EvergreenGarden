using Newtonsoft.Json;

public enum ItemType
{
    Carrot,
    Corn,
    Rice,
    Pig,
    Cow,
    Chicken,
}
public class ItemInformation
{
    public string name { get; set; }
    public int buyPrice { get; set; }
    public int sellPrice { get; set; }

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

