using Newtonsoft.Json;
using System;

[Serializable]
public class User
{
    public int id;
    public string characterName;
    public int gold;
    public int diamond;

    public User()
    {
        id = -1;
        characterName = "Default name";
        gold = 0;
        diamond = 0;
    }

    public User(int id, string characterName, int gold, int diamond)
    {
        this.id = id;
        this.characterName = characterName;
        this.gold = gold;
        this.diamond = diamond;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
