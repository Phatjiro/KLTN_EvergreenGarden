using UnityEngine;

public class ItemInformationManager: MonoBehaviour
{
    public static ItemInformationManager _instance;

    [SerializeField]
    Sprite carrotIcon;

    private static ItemInformation carrotInfo = new ItemInformation(ItemType.Carrot.ToString(), 10, 8);

    private void Start()
    {
        _instance = this;
    }

    public static ItemInformation GetItemInformation(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Carrot:
                return carrotInfo;
            case ItemType.Corn:
                return null;
            default:
                return null;
        }
    }
    public Sprite GetIcon(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Carrot:
                return carrotIcon;
            case ItemType.Corn:
                return null;
            default:
                return null;
        }
    }
}