using Newtonsoft.Json;

public class ItemInBag
{
    public ItemType type { get; set; }
    public int quantity { get; set; }

    public ItemInBag(ItemType type, int quantity)
    {
        this.type = type;
        this.quantity = quantity;
    }
    public ItemInBag() { }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}