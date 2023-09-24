using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolManager : MonoBehaviour
{
    [SerializeField]
    Button buttonDigging;
    [SerializeField]
    Button buttonWatered;

    List<Button> allButtons;
    int indexDig = 0;
    int indexWatered = 1;

    private void Awake()
    {
        buttonDigging.onClick.AddListener(ActiveDigging);
        buttonWatered.onClick.AddListener(ActiveWatering);

        allButtons = new List<Button>() { buttonDigging, buttonWatered };
    }

    private void ActiveDigging()
    {
        Debug.Log("Change or disable to digging mode");
        bool isDiggingMode = (FarmAction.currentMode == FarmMode.Digging);
        ActiveButtonByIndex(indexDig, !isDiggingMode, FarmMode.Digging);
        EventButtonManager.SetIsClickingButton(false);
    }

    private void ActiveWatering()
    {
        Debug.Log("Change or disable to watering mode");
        bool isWateringMode = (FarmAction.currentMode == FarmMode.Watering);
        ActiveButtonByIndex(indexWatered, !isWateringMode, FarmMode.Watering);
        EventButtonManager.SetIsClickingButton(false);
    }

    private void ActiveButtonByIndex(int index, bool isActive, FarmMode modeActive)
    {
        for (int i = 0; i < allButtons.Count; i++)
        {
            if (i == index)
            {
                ActiveButton(allButtons[i], isActive);
                if (isActive == false)
                {
                    FarmAction.currentMode = FarmMode.None;
                }
                else
                {
                    FarmAction.currentMode = modeActive;
                }
            }
            else
            {
                ActiveButton(allButtons[i], false);
            }
        }
    }

    private void ActiveButton(Button btn, bool isActive)
    {
        btn.gameObject.transform.GetChild(0).gameObject.SetActive(isActive);
    }
}
