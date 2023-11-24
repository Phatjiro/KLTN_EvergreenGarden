using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField]
    Button buttonPlatingCorn;
    [SerializeField]
    Button buttonPlantingRice;

    List<Button> allButtons;
    int indexDiging = 0;
    int indexWatering = 1;
    int indexPlatingGrass = 2;
    int indexPlatingCarrot = 3;
    int indexCorn = 4;
    int indexRice = 5;
    int indexGloving = 6;

    List<GameObject> allObjects;

    [SerializeField]
    GameObject glovesRight;
    [SerializeField]
    GameObject glovesLeft;
    [SerializeField]
    GameObject shovelAsset;
    [SerializeField]
    GameObject wateringCan;
    [SerializeField]
    GameObject seedBagGrass;
    [SerializeField]
    GameObject seedBagCarrot;
    [SerializeField]
    GameObject seedBagCorn;
    [SerializeField]
    GameObject seedBagRice;

    SoundButtonManager soundButtonManager;

    private void Awake()
    {
        buttonDigging.onClick.AddListener(ActiveDigging);
        buttonWatering.onClick.AddListener(ActiveWatering);
        buttonPlantingGrass.onClick.AddListener(ActivePlatingGrass);
        buttonPlantingCarrot.onClick.AddListener(ActivePlatingCarrot);
        buttonPlatingCorn.onClick.AddListener(ActivePlatingCorn);
        buttonPlantingRice.onClick.AddListener(ActivePlantingRice);
        buttonGloving.onClick.AddListener(ActiveGloving);

        allButtons = new List<Button>()
        {
            buttonDigging,
            buttonWatering,
            buttonPlantingGrass,
            buttonPlantingCarrot,
            buttonPlatingCorn,
            buttonPlantingRice,
            buttonGloving,
        };

        allObjects = new List<GameObject>()
        {
            shovelAsset,
            wateringCan,
            seedBagGrass,
            seedBagCarrot,
            seedBagCorn,
            seedBagRice,
            glovesRight,
        };
    }

    private void Start()
    {
        soundButtonManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundButtonManager>();
    }

    private void ActiveDigging()
    {
        Debug.Log("Change or disable to digging mode");
        bool isDiggingMode = (FarmAction.currentMode == FarmMode.Digging);
        ActiveButtonByIndex(indexDiging, !isDiggingMode, FarmMode.Digging);
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

    private void ActivePlatingCorn()
    {
        Debug.Log("Change or disable to planting corn");
        bool isPlatingCorn = (FarmAction.currentMode == FarmMode.PlantingCorn);
        ActiveButtonByIndex(indexCorn, !isPlatingCorn, FarmMode.PlantingCorn);
    }

    private void ActivePlantingRice()
    {
        Debug.Log("Change or disable to planting rice");
        bool isPlatingRice = (FarmAction.currentMode == FarmMode.PlantingRice);
        ActiveButtonByIndex(indexRice, !isPlatingRice, FarmMode.PlantingRice);
    }

    private void ActiveButtonByIndex(int index, bool isActive, FarmMode modeActive)
    {
        soundButtonManager.PlaySFX(soundButtonManager.select_item);
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
