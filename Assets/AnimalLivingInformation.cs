using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalLivingInformation : MonoBehaviour
{
    public Animal information { get; set; }

    private void Update()
    {
        float timePassed = (float)DateTime.Now.Subtract(information.buyTime).TotalSeconds;
        float growSize = timePassed / information.timeGrowsUp;
        growSize = Mathf.Min(growSize, 1);
        growSize = growSize * (information.maxSize - information.minSize) + information.minSize;

        gameObject.transform.localScale = Vector3.one * growSize;

        if (timePassed >= information.timeGrowsUp)
        {
            gameObject.transform.Find("Mark").gameObject.SetActive(true);
        }

        if (growSize > information.maxSize)
        {
            this.enabled = false;
        }
    }
}
