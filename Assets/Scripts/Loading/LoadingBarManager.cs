using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static string NEXT_SCENE = "";

    [SerializeField]
    GameObject loadingBox;

    [SerializeField]
    Image loadingInside;

    [SerializeField]
    Text textPercent;

    bool isLoading = false;
    public float currentPercent;


    // Start is called before the first frame update
    void Start()
    {
        LoadingInTimeAndChangeScene(5);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoading)
        { 
            SetTextPercent(loadingInside.fillAmount);
        }
    }

    public void SetPercent(float percent)
    { 
        this.currentPercent = percent;

        SetImageFill(percent);
        SetTextPercent(percent);
    }

    public void SetImageFill(float percent)
    {
        if (loadingInside != null)
        {
            loadingInside.fillAmount = percent;
        }
    }

    public void SetTextPercent(float percent)
    {
        if (textPercent != null)
        {
            textPercent.text = Mathf.RoundToInt(percent * 100) + "%";
        }
    }

    public void LoadingInTime(float time)
    {
        loadingInside.fillAmount = 0;
        isLoading = true;
        loadingInside.DOFillAmount(1, time).OnComplete(() => isLoading = false);
    }

    public void LoadingInTimeAndChangeScene(float time)
    {
        loadingInside.fillAmount = 0;
        isLoading = true;
        loadingInside.DOFillAmount(1, time).OnComplete(() =>
        {
            isLoading = false;
            SceneManager.LoadScene(NEXT_SCENE);
        });
    }
}
