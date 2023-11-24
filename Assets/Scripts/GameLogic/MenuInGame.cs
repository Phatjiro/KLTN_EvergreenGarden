using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour
{
    [SerializeField]
    Button buttonMenu;
    [SerializeField]
    GameObject menuObject;
    [SerializeField]
    Button buttonExit;
    [SerializeField]
    Button buttonHome;

    [SerializeField]
    Button buttonSettings;
    [SerializeField]
    GameObject settingsObject;

    [SerializeField]
    Button buttonQuit;

    SoundButtonManager soundButtonManager;

    private void Awake()
    {
        buttonMenu.onClick.AddListener(ShowMenuManager);
        buttonExit.onClick.AddListener(ExitMenuManager);
        buttonHome.onClick.AddListener(ShowMenuScene);
        buttonSettings.onClick.AddListener(ShowMenuSettings);
        buttonQuit.onClick.AddListener(QuitGame);
    }

    // Start is called before the first frame update
    void Start()
    {
        soundButtonManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundButtonManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMenuManager()
    {
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        menuObject.SetActive(!menuObject.activeSelf);
    }

    public void ExitMenuManager()
    {
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        if (settingsObject != null && settingsObject.activeSelf == true)
        { 
            settingsObject.SetActive(false);
        }
        menuObject.SetActive(false);
    }

    public void ShowMenuScene()
    {
        LoadingManager.NEXT_SCENE = "MenuScene";
        SceneManager.LoadScene("LoadingScene");
    }

    public void ShowMenuSettings()
    {
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        settingsObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
