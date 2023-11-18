using UnityEngine;

public class ItemInformationManager: MonoBehaviour
{
    public static ItemInformationManager _instance;

    [SerializeField]
    Sprite carrotIcon;
    [SerializeField]
    Sprite cornIcon;

    private static ItemInformation carrotInfo = new ItemInformation(ItemType.Carrot.ToString(), 2, 8);
    private static ItemInformation cornInfo = new ItemInformation(ItemType.Corn.ToString(), 4, 16);

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
                return cornInfo;
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
                return cornIcon;
            default:
                return null;
        }
    }
}