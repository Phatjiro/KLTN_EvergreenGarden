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

    [SerializeField]
    SoundButtonManager soundButtonManager;

    void Awake()
    {
        // Ontap button
        buttonPlay.onClick.AddListener(LoadPlaySceneWithLoading);
        buttonChangeAccount.onClick.AddListener(ChangeAccount);

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
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
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
}
