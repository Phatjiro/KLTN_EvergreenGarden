using UnityEngine;

public class ItemInformationManager: MonoBehaviour
{
    public static ItemInformationManager _instance;

    public static float cycleCarrot = 50;

    public static float cycleRice = 20;

    public static float cycleCorn = 80;

    [SerializeField]
    Sprite carrotIcon;
    [SerializeField]
    Sprite cornIcon;
    [SerializeField]
    Sprite riceIcon;

    private static ItemInformation carrotInfo = new ItemInformation(ItemType.Carrot.ToString(), 2, 8);
    private static ItemInformation cornInfo = new ItemInformation(ItemType.Corn.ToString(), 4, 16);
    private static ItemInformation riceInfo = new ItemInformation(ItemType.Rice.ToString(), 1, 4);

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
            case ItemType.Rice:
                return riceInfo;
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
            case ItemType.Rice:
                return riceIcon;
            default:
                return null;
        }
    }
}