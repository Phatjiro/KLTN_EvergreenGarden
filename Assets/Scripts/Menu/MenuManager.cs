using Firebase.Auth;
using Google;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    Text textHiUser;

    [SerializeField]
    Button buttonPlay;
    [SerializeField]
    Button buttonChangeAccount;

    SoundButton soundButton;

    [SerializeField]
    Button buttonVolume;
    [SerializeField]
    Image imageVolumeOn;
    [SerializeField]
    Image imageVolumeOff;

    void Awake()
    {
        // Ontap button
        buttonPlay.onClick.AddListener(LoadPlaySceneWithLoading);
        buttonChangeAccount.onClick.AddListener(ChangeAccount);
        buttonVolume.onClick.AddListener(SwitchVolumeState);

        soundButton = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundButton>();

        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            // Get DisplayName of current user to Hi ^^
            textHiUser.text = "Hi, " + FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ChangeAccount()
    {
        try
        {
            // Sign out from FirebaseAuth
            FirebaseAuth.DefaultInstance.SignOut();

            // Sign out from GoogleSignIn
            GoogleSignIn.DefaultInstance.SignOut();

            // Back to LoginScene
            Debug.Log("Sign out successful! - Load to LoginScene");
            SceneManager.LoadScene("LoginScene");
        }
        catch (Exception e)
        {
            Debug.Log("Error during sign out: " + e.Message);
        }
    }

    private void LoadPlaySceneWithLoading()
    {
        LoadingManager.NEXT_SCENE = "PlayScene";
        SceneManager.LoadScene("LoadingScene");
    }

    public void SwitchVolumeState()
    {
        if (PlayerPrefs.GetInt("SoundStatus") == 1)
        {
            soundButton.TurnOffAudio();
            imageVolumeOn.enabled = false;
            imageVolumeOff.enabled = true;
            PlayerPrefs.SetInt("SoundStatus", 0);
        }
        else
        {
            soundButton.TurnOnAudio();
            imageVolumeOff.enabled = false;
            imageVolumeOn.enabled = true;
            PlayerPrefs.SetInt("SoundStatus", 1);
        }   
    }
}
