using Firebase;
using Firebase.Auth;
using Google;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    FirebaseAuth auth;

    // Sign in google
    [SerializeField]
    Button buttonLoginWithGoogle;

    // Sign in email password
    [SerializeField]
    InputField inputFieldEmail;
    [SerializeField]
    InputField inputFieldPassword;
    [SerializeField]
    Text textNotifyLoginEmailPassword;

    // Register email password
    [SerializeField]
    InputField inputFieldRegisterEmail;
    [SerializeField]
    InputField inputFieldRegisterPassword;
    [SerializeField]
    InputField inputFieldConfirmPassword;
    [SerializeField]
    Text textNotifyRegisterEmailPassword;

    // Login
    [SerializeField]
    GameObject loginForm;
    [SerializeField]
    Button buttonLoadRegisterForm;
    [SerializeField]
    Button buttonLogin;
    [SerializeField]
    Button buttonForgotPassword;

    // Register
    [SerializeField]
    GameObject registerForm;
    [SerializeField]
    Button buttonLoadLoginForm;
    [SerializeField]
    Button buttonRegister;


    private void Awake()
    {
        // Ontap button
        buttonLoadRegisterForm.onClick.AddListener(SwitchLoginRegisterForm);
        buttonLoadLoginForm.onClick.AddListener(SwitchLoginRegisterForm);
        buttonLogin.onClick.AddListener(SignInWithEmailPassword);
        buttonRegister.onClick.AddListener(RegisterWithEmailPassword);
        buttonLoginWithGoogle.onClick.AddListener(SignInWithGoogle);
        buttonForgotPassword.onClick.AddListener(ForgetPassword);

        // Init GoogleSignInConfiguration
        GoogleSignInConfiguration configuration = new GoogleSignInConfiguration
        {
            WebClientId = "987333857141-i5mdf4rsae842joq1gnednru6duo44d8.apps.googleusercontent.com",
            RequestEmail = true,
            RequestIdToken = true,
        };

        GoogleSignIn.Configuration = configuration;

        // Check dependencies
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(async task =>
        {
            DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Init Firebase
                auth = FirebaseAuth.DefaultInstance;

                // Check if user login already
                if (auth.CurrentUser != null)
                {
                    Debug.Log("User: " + auth.CurrentUser.DisplayName + " already login - Load MenuScene");

                    SceneManager.LoadScene("MenuScene");
                    Debug.Log("Loaded scene");
                }
            }
            else
            {
                Debug.Log("Error Firebase dependencies: " + dependencyStatus);
            }
        });
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
                Debug.Log("SignInWithFirebase - was encountered an error: " + task.Exception);
                return;
            }
            if (task.IsCanceled)
            {
                Debug.Log("Firebase sign in - was canceled");
                return;
            }
            if (task.IsCompleted)
            {
                // Firebase sign in successful
                FirebaseUser user = task.Result;
                Debug.Log("Firebase user sign in: " + user.DisplayName);
                Debug.Log("Load to MenuScene");
                SceneManager.LoadScene("MenuScene");
            }
        });
    }

    public bool IsEmailValid(string email)
    {
        string pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }

    public void SignInWithEmailPassword()
    {
        string email = inputFieldEmail.text;
        string password = inputFieldPassword.text;
        Debug.Log("Your email/password: " + email + "/" + password);

        if (email == "" || password == "")
        {
            Debug.Log("Email and password can not empty");
            textNotifyLoginEmailPassword.text = "Please enter email and password";
            return;
        }

        if (!IsEmailValid(email))
        {
            Debug.Log("Your email is invalid");
            textNotifyLoginEmailPassword.text = "Your email is invalid";
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("SignInWithEmailPassword - was encountered an error: " + task.Exception);
                textNotifyLoginEmailPassword.text = "Email or password is incorrect";
                return;
            }
            if (task.IsCanceled)
            {
                Debug.Log("SignInWithEmailPassword - was canceled");
                return;
            }
            if (task.IsCompleted)
            {
                // Email password sign in successful
                FirebaseUser user = task.Result.User;
                Debug.Log("Firebase user sign in: " + user.DisplayName);
                Debug.Log("Load to MenuScene");  
                SceneManager.LoadScene("MenuScene");
            }
        });
    }

    public bool CheckConfirmPassword(string pass, string confirm)
    {
        if (pass == confirm)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RegisterWithEmailPassword()
    { 
        string email = inputFieldRegisterEmail.text;
        string password = inputFieldRegisterPassword.text;
        string confirmPassword = inputFieldConfirmPassword.text;

        if (email == "" || password == "" || confirmPassword == "")
        {
            Debug.Log("Email or password or confirmPassword empty");
            textNotifyLoginEmailPassword.text = "Email, password and confirm password can not empty";
            return;
        }

        if (password.Length < 6)
        {
            textNotifyLoginEmailPassword.text = "Your password must be longer than 6 characters";
        }

        if (!CheckConfirmPassword(password, confirmPassword))
        {
            Debug.Log("Password do not match");
            textNotifyRegisterEmailPassword.text = "Password confirm does not match";
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("RegisterWithEmailPassword - was encountered an error: " + task.Exception);
                return;
            }
            if (task.IsCanceled)
            {
                Debug.Log("RegisterWithEmailPassword - was canceled");
                return;
            }
            if (task.IsCompleted)
            {
                FirebaseUser user = task.Result.User;
                Debug.Log("User registered successfully: " + user.Email);
                Debug.Log("Load to MenuScene");
                SceneManager.LoadScene("MenuScene");
            }
        });
    }

    public void SwitchLoginRegisterForm()
    {
        loginForm.SetActive(!loginForm.activeSelf);
        registerForm.SetActive(!registerForm.activeSelf);
    }

    public void ForgetPassword()
    {
        string email = inputFieldEmail.text;

        if (email == "")
        {
            textNotifyLoginEmailPassword.text = "Please enter your email to reset password";
            return;
        }

        if (!IsEmailValid(email))
        {
            textNotifyLoginEmailPassword.text = "Your email is invalid";
            return;
        }

        auth.SendPasswordResetEmailAsync(email).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Send email reset password failed: " + task.Exception);
                textNotifyLoginEmailPassword.text = "Your email is incorrect or not registered";
                return;
            }
            if (task.IsCanceled)
            {
                Debug.Log("Send email reset password canceled");
                return;
            }
            if (task.IsCompleted)
            {
                Debug.Log("Password reset email sent successfully");
                textNotifyLoginEmailPassword.text = "Success! Please check your email to reset password.";
            }
        });
    }
}
