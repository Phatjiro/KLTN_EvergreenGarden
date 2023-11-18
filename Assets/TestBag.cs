using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBag : MonoBehaviour
{
    public static TestBag _instance;

    [SerializeField]
    Transform itemContainer;

    List<BagItemCanvasManager> allItem = new List<BagItemCanvasManager>();

    private void Start()
    {
        Debug.Log("Intance BagUIManager");
        _instance = this;

        foreach (Transform child in itemContainer)
        {
            if (child.GetComponent<BagItemCanvasManager>() != null)
                allItem.Add(child.GetComponent<BagItemCanvasManager>());
        }
    }

    public void UpdateItemAt(int index, Sprite icon, int quantity)
    {
        allItem[index].SetIcon(icon);
        allItem[index].SetQuantity(quantity);
    }
}
