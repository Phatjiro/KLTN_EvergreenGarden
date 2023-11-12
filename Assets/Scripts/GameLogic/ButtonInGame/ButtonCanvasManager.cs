using UnityEngine;
using UnityEngine.UI;

public class ButtonCanvasManager : MonoBehaviour
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

    public void ShowBag()
    {
        bagObject.SetActive(!bagObject.activeSelf);
    }

    public void ExitBag()
    {
        bagObject.SetActive(false);
    }
}
