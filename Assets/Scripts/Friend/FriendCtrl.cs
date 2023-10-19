
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

[Serializable]
public class Player
{
    public Sprite Avt { get; set; }
    public string Name { get; set; }
    public string ID { get; set; }

    public Player(Sprite avt, string name, string iD)
    {
        Avt = avt;
        Name = name;
        ID = iD;
    }
}

public class FriendCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject template = transform.GetChild(0).gameObject;
        GameObject g;
        List<Player> listplayer = new List<Player>();
        listplayer.Add(new Player(null, "name 1", "id 1"));
        listplayer.Add(new Player(null, "name 2", "id 2"));
        listplayer.Add(new Player(null, "name 3", "id 3"));
        listplayer.Add(new Player(null, "name 4", "id 4"));
        for (int i = 0; i < listplayer.Count; i++)
        {
            g = Instantiate(template, transform);
            g.transform.GetChild(1).GetComponent<Image>().sprite = listplayer[i].Avt;
            g.transform.GetChild(2).GetComponent<Text>().text = listplayer[i].Name;
            g.transform.GetChild(3).GetComponent<Text>().text = listplayer[i].ID;
            if(i%2 == 0)
            {
                g.transform.GetChild(0).GetComponent<Image>().color = Color.HSVToRGB(0.11f, 0.52f, 1f);
            }
            else
            {
                g.transform.GetChild(0).GetComponent<Image>().color = Color.HSVToRGB(0.11f, 0.63f, 0.95f);
            }
        }
        Destroy(template);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
