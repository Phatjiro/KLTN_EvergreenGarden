using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventButtonManager : MonoBehaviour
{
    private static bool isClickingButton = false;

    public static void SetIsClickingButton(bool isClicking)
    { 
        isClickingButton = isClicking;
    }

    public static bool GetIsClickingButton()
    {
        return isClickingButton;
    }
}