using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    Button buttonChangeAccount;

    private void Awake()
    {
        // Ontap button
        buttonChangeAccount.onClick.AddListener(ChangeAccount);
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
        // Sign out from FirebaseAuth
        FirebaseAuth.DefaultInstance.SignOut();

        // Sign out from GoogleSignIn
        GoogleSignIn.DefaultInstance.SignOut();

        // Back to LoginScene
        SceneManager.LoadScene("LoginScene");
    }
}
