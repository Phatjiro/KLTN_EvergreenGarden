using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    GameObject shopObject;
    [SerializeField]
    Button buttonExitShop;

    [SerializeField]
    GameObject bagObject;
    [SerializeField]
    Button buttonExitBag;
    [SerializeField]
    GameObject sellPriceObject;

    [SerializeField]
    GameObject wizardShop;
    [SerializeField]
    Button buttonWizardPurchase;
    [SerializeField]
    Button buttonWizardSell;

    [SerializeField]
    GameObject seedShopObject;

    private void Awake()
    {
        buttonWizardPurchase.onClick.AddListener(ActiveShop);
        buttonExitShop.onClick.AddListener(ExitShop);
        buttonWizardSell.onClick.AddListener(ActiveSellBag);
        buttonExitBag.onClick.AddListener(ExitPriceObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // User touch on screen
            if (touch.phase == TouchPhase.Began)
            {
                if (EventButtonManager.GetIsClickingButton()) return;

                // Use ray to check if user click on Shop
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                if (hit.collider != null && hit.collider.gameObject.tag == "Shop")
                {
                    ActiveWizard();
                }
            }
        }    
        
    }

    public void ActiveWizard()
    {
        if (shopObject.activeSelf != true && bagObject.activeSelf != true && seedShopObject.activeSelf != true)
        {
            wizardShop.SetActive(true);
        }
    }

    public void ExitWizard()
    {
        wizardShop.SetActive(false);
    }

    public void ActiveShop()
    {
        wizardShop.SetActive(false);
        shopObject.SetActive(true);
    }

    public void ExitShop()
    {
        shopObject.SetActive(false);
    }

    public void ActiveSellBag()
    {
        wizardShop.SetActive(false);
        bagObject.SetActive(true);
        MoveSellBag(-320);
        sellPriceObject.SetActive(true);
    }

    public void ExitPriceObject()
    {
        if (sellPriceObject != null && sellPriceObject.activeSelf == true)
        {
            sellPriceObject.SetActive(false);
            MoveSellBag(320);
        }
    }

    public void MoveSellBag(int x)
    {
        /*Vector3 currentPosition = bagObject.transform.position;
        Debug.Log(bagObject.transform.position.x);
        currentPosition.x += x;
        bagObject.transform.position = currentPosition;
        Debug.Log(bagObject.transform.position.x);*/

        Vector2 currentPos = bagObject.GetComponent<RectTransform>().anchoredPosition;
        Debug.Log("CurrentPos: " + currentPos);
        currentPos.x += x;
        bagObject.GetComponent<RectTransform>().anchoredPosition = currentPos;
    }
}
