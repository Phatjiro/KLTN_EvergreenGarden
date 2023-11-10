using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagUIManager : MonoBehaviour
{
    public static BagUIManager _instance;

    [SerializeField]
    Transform itemContainer;

    List<BagItemUIManager> allItem = new List<BagItemUIManager>();

    private void Start()
    {
        Debug.Log("Intance BagUIManager");
        _instance = this;

        foreach (Transform child in itemContainer)
        {
            if (child.GetComponent<BagItemUIManager>() != null)
                allItem.Add(child.GetComponent<BagItemUIManager>());
        }
    }

    public void UpdateItemAt(int index, Sprite icon, int quantity)
    {
        allItem[index].SetIcon(icon);
        allItem[index].SetQuantity(quantity);
    }
}
