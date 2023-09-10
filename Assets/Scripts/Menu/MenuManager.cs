using Firebase.Auth;
using Google;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    Button buttonChangeAccount;
    [SerializeField]
    Text textHiUser;

    private void Awake()
    {
        // Ontap button
        buttonChangeAccount.onClick.AddListener(ChangeAccount);

        // Get DisplayName of current user to Hi ^^
        textHiUser.text = "Hi, " + FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeAccount()
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
}
