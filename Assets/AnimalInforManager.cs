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
    Text txttimeRemaining;
    [SerializeField]
    Text txtanimalName;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadAnimalInfor(Animal animal)
    {
        txtanimalName.text = animal.name;
        txtBuyPrice.text = animal.buyPrice.ToString();
        txtSellPrice.text = animal.sellPrice.ToString();
        txttimeRemaining.text = animal.timeGrowsUp.ToString();

    }
}
