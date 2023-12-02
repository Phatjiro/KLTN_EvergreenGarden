using PolyAndCode.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (this.lstFriend == null)
        {
            Debug.Log("LstFriend null");
            return;
        }
        item.ConfigureCell(this.lstFriend[index], index);
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

#if UNITY_EDITOR
        List<User> lst = new List<User>();
        //Demo list
        for (int i = 0; i < 50; i++)
        {
            User user = new User();
            user.characterName = "User_" + i;
            user.id = ""+i;
            lst.Add(user);
        }
        SetLstItem(lst);
        ReloadUI();
#endif
    }

    private void OnEnable()
    {
        //ReloadUI();        
    }

    public void SetLstItem(List<User> lst)
    {
        this.lstFriend = lst;
    }

}
