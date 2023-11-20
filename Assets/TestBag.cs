using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBag : MonoBehaviour
{
    public static bool isAllowToSell = false;
    public static TestBag _instance;

    [SerializeField]
    Transform itemContainer;

    List<BagItemCanvasManager> allItem = new List<BagItemCanvasManager>();

    [SerializeField]
    Button buttonBag;
    [SerializeField]
    Button buttonExitBag;

    private void Awake()
    {
        buttonBag.onClick.AddListener(TurnOnOffBag);
        buttonExitBag.onClick.AddListener(CloseBag);
    }

    private void Start()
    {
        Debug.Log("Intance BagUIManager");
        _instance = this;

        foreach (Transform child in itemContainer)
        {
            if (child.GetComponent<BagItemCanvasManager>() != null)
                allItem.Add(child.GetComponent<BagItemCanvasManager>());
        }

        gameObject.SetActive(false);
    }

    public void UpdateItemAt(int index, Sprite icon, int quantity)
    {
        allItem[index].SetIcon(icon);
        allItem[index].SetQuantity(quantity);
    }

    public void ShowBag(bool isAllowToSell)
    { 
        TestBag.isAllowToSell = isAllowToSell;
        gameObject.SetActive(true);
    }

    public void CloseBag()
    {
        gameObject.SetActive(false);
    }

    public void TurnOffAllowSell()
    {
        isAllowToSell = false;
    }

    public void TurnOnOffBag()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
