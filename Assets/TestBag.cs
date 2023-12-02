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

    SoundButtonManager soundButtonManager;

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
        soundButtonManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundButtonManager>();
        Debug.Log("Intance BagUIManager");
        _instance = this;
    }

    public void ShowBag(bool isAllowToSell)
    {
        isShowing = true;
        TestBag.isAllowToSell = isAllowToSell;
        this.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f);

        CharacterActionController.isAllowToMove = false;
    }

    public void CloseBag()
    {
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        isShowing = false;
        this.GetComponent<RectTransform>().DOAnchorPosY(1000, 0.5f);

        CharacterActionController.isAllowToMove = true;
    }

    public void TurnOffAllowSell()
    {
        isAllowToSell = false;
    }

    public void TurnOnOffBag()
    {
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        if (isShowing)
            CloseBag();
        else
            ShowBag(false);
    }
}
