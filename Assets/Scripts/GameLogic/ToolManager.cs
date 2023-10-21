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
    [SerializeField]
    Button buttonGloving;

    List<Button> allButtons;
    int indexDiging = 0;
    int indexWatering = 1;
    int indexPlatingGrass = 2;
    int indexPlatingCarrot = 3;
    int indexGloving = 4;

    List<GameObject> allObjects;

    [SerializeField]
    GameObject glovesRight;
    [SerializeField]
    GameObject glovesLeft;
    [SerializeField]
    GameObject shovelAsset;

    private void Awake()
    {
        buttonDigging.onClick.AddListener(ActiveDigging);
        buttonWatering.onClick.AddListener(ActiveWatering);
        buttonPlantingGrass.onClick.AddListener(ActivePlatingGrass);
        buttonPlantingCarrot.onClick.AddListener(ActivePlatingCarrot);
        buttonGloving.onClick.AddListener(ActiveGloving);

        allButtons = new List<Button>() 
        { 
            buttonDigging,
            buttonWatering,
            buttonPlantingGrass,
            buttonPlantingCarrot,
            buttonGloving
        };

        allObjects = new List<GameObject>()
        { 
            shovelAsset,
            null,
            null,
            null,
            glovesRight,
        };
    }

    private void ActiveDigging()
    {
        Debug.Log("Change or disable to digging mode");
        bool isDiggingMode = (FarmAction.currentMode == FarmMode.Digging);
        ActiveButtonByIndex(indexDiging, !isDiggingMode, FarmMode.Digging);
        //EventButtonManager.SetIsClickingButton(false);
    }

    private void ActiveWatering()
    {
        Debug.Log("Change or disable to watering mode");
        bool isWateringMode = (FarmAction.currentMode == FarmMode.Watering);
        ActiveButtonByIndex(indexWatering, !isWateringMode, FarmMode.Watering);
    }

    private void ActivePlatingGrass()
    {
        Debug.Log("Change or disable to plating grass");
        bool isPlatingGrassMode = (FarmAction.currentMode == FarmMode.PlantingGrass);
        ActiveButtonByIndex(indexPlatingGrass, !isPlatingGrassMode, FarmMode.PlantingGrass);
    }

    private void ActivePlatingCarrot()
    {
        Debug.Log("Change or disable to plating carrot");
        bool isPlatingCarrotMode = (FarmAction.currentMode == FarmMode.PlantingCarrot);
        ActiveButtonByIndex(indexPlatingCarrot, !isPlatingCarrotMode, FarmMode.PlantingCarrot);
    }

    private void ActiveGloving()
    {
        Debug.Log("Change or disable to gloving");
        bool isGlovingMode = (FarmAction.currentMode == FarmMode.Gloving);
        ActiveButtonByIndex(indexGloving, !isGlovingMode, FarmMode.Gloving);
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
                    allObjects[i].SetActive(false);
                }
                else
                {
                    FarmAction.currentMode = modeActive;
                    allObjects[i].SetActive(true);
                }
            }
            else
            {
                ActiveButton(allButtons[i], false);
                allObjects[i]?.SetActive(false);
            }
        }
        glovesLeft.SetActive(glovesRight.activeSelf);
    }

    private void ActiveButton(Button btn, bool isActive)
    {
        btn.gameObject.transform.GetChild(0).gameObject.SetActive(isActive);
    }
}
