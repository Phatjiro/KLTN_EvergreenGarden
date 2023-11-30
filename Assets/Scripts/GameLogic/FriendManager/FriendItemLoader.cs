using PolyAndCode.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendItemLoader : MonoBehaviour, IRecyclableScrollRectDataSource
{
    public static FriendItemLoader Instance;

    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    [SerializeField]
    private int _dataLength;

    // Data list
    private List<User> lstFriend = new List<User>();

    private void Awake()
    {
        _recyclableScrollRect.DataSource = this;
        ReloadUI();
    }

    public void ReloadUI()
    {
        Debug.Log("Reload friend list");
        _recyclableScrollRect.ReloadData();
    }

    public int GetItemCount()
    {
        return lstFriend.Count;
    }

    public void SetCell(ICell cell, int index)
    {
        //Casting to the implemented Cell
        var item = cell as FriendItemCanvasManager;
        item.ConfigureCell(this.lstFriend[index], index);
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        ReloadUI();        
    }

    public void SetLstItem(List<User> lst)
    {
        this.lstFriend = lst;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
