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

    SoundButtonManager soundButtonManager;

    private void Awake()
    {
        buttonChangeName.onClick.AddListener(OpenWizardCharacterName);
    }

    private void Start()
    {
        soundButtonManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundButtonManager>();
    }

    public void OpenWizardCharacterName()
    {
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        wizardCharacterName.SetActive(true);
        menuObject.SetActive(false);
    }
}
