using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagItemUIManager : MonoBehaviour
{
    [SerializeField]
    Image iconDisplay;

    [SerializeField]
    Text quantityDisplay;

    public void SetIcon(Sprite sprite)
    {
        iconDisplay.sprite = sprite;
        iconDisplay.color = Color.white;
    }
    public void SetQuantity(int quantity)
    {
        this.quantityDisplay.text = quantity.ToString();
    }

    public void SetCarrot()
    {

    }
}
