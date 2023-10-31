using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
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

    override
    public string ToString()
    {
        return $"{id},{characterName},{gold},{diamond}";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
