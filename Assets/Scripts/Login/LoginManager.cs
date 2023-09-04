using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    private FirebaseAuth auth;

    [SerializeField]
    Button buttonLoginWithGoogle;

    private void Awake()
    {
        // Ontap button
        buttonLoginWithGoogle.onClick.AddListener(SignInWithGoogle);

        // Init GoogleSignInConfiguration
        GoogleSignInConfiguration configuration = new GoogleSignInConfiguration
        {
            WebClientId = "987333857141-i5mdf4rsae842joq1gnednru6duo44d8.apps.googleusercontent.com",
            RequestEmail = true,
            RequestIdToken = true,
        };

        GoogleSignIn.Configuration = configuration;

        // Check dependencies
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Init Firebase
                auth = FirebaseAuth.DefaultInstance;

                // Check if user login already
                if (auth.CurrentUser != null)
                {
                    Debug.Log("User: " + auth.CurrentUser.DisplayName + " already login - Load MenuScene");
                    SceneManager.LoadScene("MenuScene");
                }
            }
            else
            {
                Debug.Log("Error Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SignInWithGoogle()
    {
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Login failed
                Debug.Log("GoogleSignIn failed: " + task.Exception);
                return;
            }
            if (task.IsCanceled)
            {
                // Login canceled
                Debug.Log("GoogleSignIn canceled");
                return;
            }
            else
            { 
                // Get the GoogleSignIn result
                GoogleSignInUser signInUser = task.Result;
                string idToken = signInUser.IdToken;

                SignInWithFirebase(idToken);
            }
        });
    }
    public void SignInWithFirebase(string idToken)
    {
        // Sign in to Firebase with Google Token Id
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Firebase sign in was encountered an error: " + task.Exception);
                return;
            }
            if (task.IsCanceled)
            {
                Debug.Log("Firebase sign in was canceled");
                return;
            }
            if (task.IsCompleted)
            {
                // Firebase sign in successful
                FirebaseUser firebaseUser = task.Result;
                Debug.Log("Firebase user sign in: " + firebaseUser.DisplayName);
                Debug.Log("Load to MenuScene");
                SceneManager.LoadScene("MenuScene");
            }
        });
    }
}
