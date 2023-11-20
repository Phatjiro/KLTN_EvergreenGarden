using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBag : MonoBehaviour
{
    public static bool isAllowToSell = false;
    public static TestBag _instance;

    [SerializeField]
    Transform itemContainer;

    List<BagItemCanvasManager> allItem = new List<BagItemCanvasManager>();

    [SerializeField]
    Button buttonBag;
    [SerializeField]
    Button buttonExitBag;

    private static bool isShowing = false;

    private void Awake()
    {
        buttonBag.onClick.AddListener(TurnOnOffBag);
        buttonExitBag.onClick.AddListener(CloseBag);
    }

    public static bool IsShowing()
    {
        return isShowing;
    }

    private void Start()
    {
        Debug.Log("Intance BagUIManager");
        _instance = this;
    }

    public void ShowBag(bool isAllowToSell)
    {
        isShowing = true;
        TestBag.isAllowToSell = isAllowToSell;
        this.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f);
    }

    public void CloseBag()
    {
        isShowing = false;
        this.GetComponent<RectTransform>().DOAnchorPosY(1000, 0.5f);
    }

    public void TurnOffAllowSell()
    {
        isAllowToSell = false;
    }

    public void TurnOnOffBag()
    {
        if (isShowing)
            CloseBag();
        else
            ShowBag(false);
    }
}
