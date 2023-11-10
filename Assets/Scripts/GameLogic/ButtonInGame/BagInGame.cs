using UnityEngine;
using UnityEngine.UI;

public class BagInGame : MonoBehaviour
{
    [SerializeField]
    Button buttonBag;
    [SerializeField]
    GameObject bagObject;
    [SerializeField]
    Button buttonExit;

    private void Awake()
    {
        buttonBag.onClick.AddListener(ShowBag);
        buttonExit.onClick.AddListener(ExitBag);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (bagObject.activeSelf == true)
        { 
            ShowBag();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowBag()
    {
        bagObject.SetActive(!bagObject.activeSelf);
    }

    public void ExitBag()
    {
        bagObject.SetActive(false);
    }
}
