using PimDeWitte.UnityMainThreadDispatcher;
using PolyAndCode.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BagItemLoader : MonoBehaviour, IRecyclableScrollRectDataSource
{
    public static BagItemLoader instance;

    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    [SerializeField]
    private int _dataLength;
    
    // Data list
    private List<ItemInBag> lstItemInBag = new List<ItemInBag>();

    private void Awake()
    {
        _recyclableScrollRect.DataSource = this;
        ReloadUI();
    }

    private void Start()
    {
        instance = this;
    }
    private void OnEnable()
    {
        ReloadUI();
    }
    public void SetLstItem(List<ItemInBag> lst)
    {
        this.lstItemInBag = lst;
    }
    public void ReloadUI()
    {
        Debug.Log("Reload bag");
        _recyclableScrollRect.ReloadData();
    }

    public void AddItemToLst(ItemInBag item)
    {
        Debug.Log("Add item: " + item.type);
        for (int i = 0; i < lstItemInBag.Count; i++)
        {
            if (lstItemInBag[i].type == item.type)
            {
                lstItemInBag[i].quantity += item.quantity;
                return;
            }
        }
        lstItemInBag.Add(item);
    }
    public int GetItemCount()
    {
        return lstItemInBag.Count;
    }

    public void SetCell(ICell cell, int index)
    {
        //Casting to the implemented Cell
        var item = cell as BagItemCanvasManager;
        item.ConfigureCell(this.lstItemInBag[index], index);
    }
}

