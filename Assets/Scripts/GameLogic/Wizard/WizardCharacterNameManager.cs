using UnityEngine;
using UnityEngine.UI;

public class WizardCharacterNameManager : MonoBehaviour
{
    [SerializeField]
    Button buttonSubmit;

    [SerializeField]
    InputField inputFieldCharacterName;

    [SerializeField]
    UserLoaderManager userLoaderManager;

    SoundButtonManager soundButtonManager;

    private void Awake()
    {
        buttonSubmit.onClick.AddListener(SetCharacterName);
    }

    private void Start()
    {
        soundButtonManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundButtonManager>();
    }

    private void OnEnable()
    {
        CharacterActionController.isAllowToMove = false;
    }

    private void OnDisable()
    {
        CharacterActionController.isAllowToMove = true;
    }

    public void SetCharacterName()
    {
        if (inputFieldCharacterName.text == "")
        {
            soundButtonManager.PlaySFX(soundButtonManager.failed);
            string mess = "Nah! Please enter a name";
            inputFieldCharacterName.placeholder.GetComponent<Text>().text = mess;
            if (NotificationManager.instance == null) return;
            NotificationManager.instance.ShowNotification(mess, 5);
        }
        else
        {
            soundButtonManager.PlaySFX(soundButtonManager.success);
            userLoaderManager.SetDisplayName(inputFieldCharacterName.text);
            this.gameObject.SetActive(false);
        }
    }
}
