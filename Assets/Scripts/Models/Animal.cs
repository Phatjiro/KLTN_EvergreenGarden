using Newtonsoft.Json;
using System;

[Serializable]
public class Animal 
{
    public string name {  get; set; }
    public DateTime buyTime { get; set; }
    public int timeGrowsUp { get; set; }
    public int buyPrice { get; set; }
    public int sellPrice { get; set; }

    public Animal(string name, DateTime buyTime, int timeGrowsUp, int buyPrice, int sellPrice)
    {
        this.name = name;
        this.buyTime = buyTime;
        this.timeGrowsUp = timeGrowsUp;
        this.buyPrice = buyPrice;
        this.sellPrice = sellPrice;
    }
    public Animal() { }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
