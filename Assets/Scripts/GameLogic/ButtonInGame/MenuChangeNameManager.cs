using UnityEngine;
using UnityEngine.UI;

public class MenuChangeNameManager : MonoBehaviour
{
    [SerializeField]
    GameObject wizardCharacterName;
    [SerializeField]
    GameObject menuObject;
    [SerializeField]
    Button buttonChangeName;

    private void Awake()
    {
        buttonChangeName.onClick.AddListener(OpenWizardCharacterName);
    }

    public void OpenWizardCharacterName()
    { 
        wizardCharacterName.SetActive(true);
        menuObject.SetActive(false);
    }
}
