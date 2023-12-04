using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class EnergryDBData
{
    public int energry;
    public DateTime timeRegent;

    public EnergryDBData(int e, DateTime d)
    {
        energry = e;
        timeRegent = d;
    }
}

public class EnergyManager : MonoBehaviour
{
    public PlayerPrefs playerRefEnergy;
    public PlayerPrefs playerRefDatetime;

    public static string keyDB = "ENERGRYDB";

    public static int energy = 10;

    private static int recoverTime = 30;

    public static DateTime timeRegent;

    public static bool isRegent=false;

    [SerializeField]
    public Text textEngergy;

    static EnergyManager Instance;
    EnergryDBData data;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        if (PlayerPrefs.HasKey(keyDB))
        {

            string json = PlayerPrefs.GetString(keyDB);
            Debug.Log("Start Pref: " + json); ;
            data = JsonConvert.DeserializeObject< EnergryDBData>(json);

            float deltaTime = (float)DateTime.Now.Subtract(data.timeRegent).TotalSeconds;
            int addEnergry = Mathf.RoundToInt(deltaTime / recoverTime);
            data.energry += addEnergry;
        }
        else
        {
            Debug.Log("data new");
            data = new EnergryDBData(10, DateTime.Now);
        }

        energy = data.energry;
        timeRegent = data.timeRegent;

        textEngergy.text = energy + " / 10";
    }

    // Update is called once per frame
    void Update()
    {
        if(DateTime.Now >= timeRegent)
        {
            timeRegent = DateTime.Now.AddSeconds(recoverTime);
            data.timeRegent = timeRegent;
            plusEnergy();
        }
    }

    public static void subTractEnergy()
    {
        if(energy > 0)
        {
            energy -= 1;
            if(!isRegent)
            {
                timeRegent = DateTime.Now.AddSeconds(30);
                isRegent = true;
                Instance.enabled = true;
            }
        }
        Instance.textEngergy.text = energy + " / 10";

        Instance.data.energry = energy;
        Instance.data.timeRegent = timeRegent;
        PlayerPrefs.SetString(keyDB, JsonConvert.SerializeObject(Instance.data));

    }

    public static void plusEnergy()
    {
        if (energy < 10)
        {
            energy += 1;
        }
        if (energy == 10)
        {
            isRegent = false;
            Instance.enabled = false;
        }


        Instance.textEngergy.text = energy + " / 10";
        Instance.data.energry = energy;
        Instance.data.timeRegent = timeRegent;
        PlayerPrefs.SetString(keyDB, JsonConvert.SerializeObject(Instance.data));
    }
}
