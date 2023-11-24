using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalInforManager : MonoBehaviour
{
    [SerializeField]
    GameObject animalInfor;
    [SerializeField]
    Button btnExit;
    [SerializeField]
    Text txtBuyPrice;
    [SerializeField]
    Text txtSellPrice;
    
    [SerializeField]
    Text txtanimalName;

    [SerializeField] LoadingBarManager loading;

    Animal crrAnimal;

    [SerializeField]
    Button btnSell;

    bool isLoading = false;

    private void Awake()
    {
        btnExit.onClick.AddListener(disableUI);
    }
    private void disableUI()
    {
        animalInfor.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        btnSell.interactable = false;
    }
    private void OnDisable()
    {
        isLoading = false;
        btnSell.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLoading) return;
        double crrTimePass = DateTime.Now.Subtract(crrAnimal.buyTime).TotalSeconds;
        float crrPercent = (float)crrTimePass / crrAnimal.timeGrowsUp;
        //Debug.Log("Crr percent: " + crrPercent);
        if (crrPercent >= 1)
        {
            Debug.Log("Enough time");
            btnSell.interactable = true;
            isLoading = false;
        }
        loading.SetPercent(crrPercent);
    }

    public void loadAnimalInfor(Animal animal)
    {
        btnSell.interactable = false;
        this.crrAnimal = animal;
        txtanimalName.text = animal.name;
        txtBuyPrice.text = animal.buyPrice.ToString();
        txtSellPrice.text = animal.sellPrice.ToString();
        Debug.Log("Load animal: " + animal.ToString());
        double crrTimePass = DateTime.Now.Subtract(animal.buyTime).TotalSeconds;
        float crrPercent = (float)crrTimePass / animal.timeGrowsUp;
        loading.SetPercent(crrPercent);
        isLoading = true;
    }
}
