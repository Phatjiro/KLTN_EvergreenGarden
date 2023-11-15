using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager instance;

    [SerializeField]
    Text textMessageContent;
    private void Start()
    {
        instance = this;
    }
    public void ShowNotification(string messContent, int timeShowNotification)
    {
        this.gameObject.SetActive(true);
        textMessageContent.text = messContent;
        this.GetComponent<RectTransform>().DOAnchorPosY(-100, 1);
        this.GetComponent<Image>().DOFade(1, timeShowNotification).OnComplete(() =>
        {
            this.GetComponent<RectTransform>().DOAnchorPosY(100, 1);
        });
    }
}
