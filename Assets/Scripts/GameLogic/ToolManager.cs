using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolManager : MonoBehaviour
{
    [SerializeField]
    Button buttonDigging;
    [SerializeField]
    Button buttonWatering;
    [SerializeField]
    Button buttonPlantingGrass;
    [SerializeField]
    Button buttonPlantingCarrot;

    List<Button> allButtons;
    int indexDiging = 0;
    int indexWatering = 1;
    int indexPlatingGrass = 2;
    int indexPlatingCarrot = 3;

    private void Awake()
    {
        buttonDigging.onClick.AddListener(ActiveDigging);
        buttonWatering.onClick.AddListener(ActiveWatering);
        buttonPlantingGrass.onClick.AddListener(ActivePlatingGrass);
        buttonPlantingCarrot.onClick.AddListener(ActivePlatingCarrot);

        allButtons = new List<Button>() { buttonDigging, buttonWatering, buttonPlantingGrass, buttonPlantingCarrot };
    }

    private void ActiveDigging()
    {
        Debug.Log("Change or disable to digging mode");
        bool isDiggingMode = (FarmAction.currentMode == FarmMode.Digging);
        ActiveButtonByIndex(indexDiging, !isDiggingMode, FarmMode.Digging);
        EventButtonManager.SetIsClickingButton(false);
    }

    private void ActiveWatering()
    {
        Debug.Log("Change or disable to watering mode");
        bool isWateringMode = (FarmAction.currentMode == FarmMode.Watering);
        ActiveButtonByIndex(indexWatering, !isWateringMode, FarmMode.Watering);
        EventButtonManager.SetIsClickingButton(false);
    }

    private void ActivePlatingGrass()
    {
        Debug.Log("Change or disable to plating grass");
        bool isPlatingGrassMode = (FarmAction.currentMode == FarmMode.PlantingGrass);
        ActiveButtonByIndex(indexPlatingGrass, !isPlatingGrassMode, FarmMode.PlantingGrass);
        EventButtonManager.SetIsClickingButton(false);
    }

    private void ActivePlatingCarrot()
    {
        Debug.Log("Change or disable to plating carrot");
        bool isPlatingCarrotMode = (FarmAction.currentMode == FarmMode.PlantingCarrot);
        ActiveButtonByIndex(indexPlatingCarrot, !isPlatingCarrotMode, FarmMode.PlantingCarrot);
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
