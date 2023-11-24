using Firebase;
using Firebase.Auth;
using Google;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    // Firebase
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
    Button buttonLoginWithEmailPassword;
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
    Button buttonRegisterEmailPassword;
    [SerializeField]
    Text textNotifyRegisterEmailPassword;

    // Login form
    [SerializeField]
    GameObject loginForm;
    [SerializeField]
    Button buttonLoadRegisterForm;
    [SerializeField]
    Button buttonForgotPassword;

    // Register form
    [SerializeField]
    GameObject registerForm;
    [SerializeField]
    Button buttonLoadLoginForm;

    // Firebase write database
    [SerializeField]
    FirebaseWriteData firebaseWriteData;

    // Effect Sound
    [SerializeField]
    SoundButtonManager soundButtonManager;

    private void Awake()
    {
        // Ontap button
        buttonLoadRegisterForm.onClick.AddListener(SwitchLoginRegisterForm);
        buttonLoadLoginForm.onClick.AddListener(SwitchLoginRegisterForm);

        buttonLoginWithEmailPassword.onClick.AddListener(SignInWithEmailPassword);
        buttonRegisterEmailPassword.onClick.AddListener(RegisterWithEmailPassword);
        buttonForgotPassword.onClick.AddListener(ForgetPassword);

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
            DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Init Firebase
                auth = FirebaseAuth.DefaultInstance;

                // Check if user login already
                if (auth.CurrentUser != null)
                {
                    Debug.Log("User: " + auth.CurrentUser.DisplayName + " already login -> Load MenuScene");
                    SceneManager.LoadScene("MenuScene");
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
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Login failed
                Debug.Log("GoogleSignIn - was encountered an error: " + task.Exception);
                return;
            }
            if (task.IsCanceled)
            {
                // Login canceled
                Debug.Log("GoogleSignIn - was canceled");
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
                Debug.Log("SignInWithFirebase - was canceled");
                return;
            }
            if (task.IsCompleted)
            {
                // Firebase sign in successful
                FirebaseUser user = task.Result;

                if (user.Metadata.CreationTimestamp == auth.CurrentUser.Metadata.LastSignInTimestamp)
                {
                    // Create new User in game when complete register
                    Bag userBag = new Bag();
                    Map userMap = new Map();
                    User newUser = new User(user.UserId, "", 100, 50, userBag);
                    firebaseWriteData.WriteData("Users/" + newUser.id, newUser.ToString());
                    firebaseWriteData.WriteData("Maps/" + newUser.id, userMap.ToString());
                    Debug.Log("User created successfully");
                }

                Debug.Log("Firebase user sign in: " + user.DisplayName + " -> Load to MenuScene");
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
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        string email = inputFieldEmail.text;
        string password = inputFieldPassword.text;

        // Check empty field
        if (email == "" || password == "")
        {
            Debug.Log("Email and password can not empty");
            textNotifyLoginEmailPassword.text = "Please enter email and password";
            return;
        }

        // Check valid email
        if (!IsEmailValid(email))
        {
            Debug.Log("Your email is invalid");
            textNotifyLoginEmailPassword.text = "Your email is invalid";
            return;
        }

        // Sign in with email password
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
                textNotifyLoginEmailPassword.text = "Sign in task was canceled";
                return;
            }
            if (task.IsCompleted)
            {
                // Email password sign in successful
                FirebaseUser user = task.Result.User;
                Debug.Log("Email password user sign in: " + user.DisplayName + " -> Load to MenuScene");
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
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        string email = inputFieldRegisterEmail.text;
        string password = inputFieldRegisterPassword.text;
        string confirmPassword = inputFieldConfirmPassword.text;

        // Check empty field
        if (email == "" || password == "" || confirmPassword == "")
        {
            textNotifyRegisterEmailPassword.text = "Email, password and confirm password can not empty";
            return;
        }

        // Check valid email
        if (!IsEmailValid(email))
        {
            Debug.Log("Your email is invalid");
            textNotifyRegisterEmailPassword.text = "Your email is invalid";
            return;
        }

        // Check length password
        if (password.Length < 6)
        {
            textNotifyRegisterEmailPassword.text = "Your password must be longer than 6 characters";
            return;
        }

        // Check confirm password
        if (!CheckConfirmPassword(password, confirmPassword))
        {
            Debug.Log("Password do not match");
            textNotifyRegisterEmailPassword.text = "Password confirm does not match";
            return;
        }

        // Register with email password
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                FirebaseException exception = task.Exception.GetBaseException() as FirebaseException;
                if (exception != null)
                {
                    Debug.Log("CODEEEEEEEEEEEEEEEE ---------: " + exception.ErrorCode);
                    // ErrorCode: 8 is "Email Exist"
                    if (exception.ErrorCode == 8)
                    {
                        Debug.Log("Email already exits");
                        textNotifyRegisterEmailPassword.text = "Email already used for account creation";
                    }
                    else
                    {
                        Debug.Log("Error: " + exception);
                        textNotifyRegisterEmailPassword.text = "Register was encountered an error";
                    }
                }
                
                return;
            }
            if (task.IsCanceled)
            {
                Debug.Log("RegisterWithEmailPassword - was canceled");
                textNotifyRegisterEmailPassword.text = "Create user task was canceled";
                return;
            }
            if (task.IsCompleted)
            {
                FirebaseUser user = task.Result.User;
                Debug.Log("Email password user sign in: " + user.DisplayName + " -> Load to MenuScene");

                // Create new User in game when complete register
                Bag userBag = new Bag();
                Map userMap = new Map();
                User newUser = new User(user.UserId, "", 100, 50, userBag);
                firebaseWriteData.WriteData("Users/" + newUser.id, newUser.ToString());
                firebaseWriteData.WriteData("Maps/" + newUser.id, userMap.ToString());
                Debug.Log("User created successfully");

                SceneManager.LoadScene("MenuScene");
            }
        });
    }

    public void SwitchLoginRegisterForm()
    {
        soundButtonManager.PlaySFX(soundButtonManager.clickButton);
        loginForm.SetActive(!loginForm.activeSelf);
        registerForm.SetActive(!registerForm.activeSelf);

        if (loginForm.activeSelf == true)
        {
            inputFieldRegisterEmail.text = "";
            inputFieldRegisterPassword.text = "";
            inputFieldConfirmPassword.text = "";
        }
        if (registerForm.activeSelf == true)
        {
            inputFieldEmail.text = "";
            inputFieldPassword.text = "";
        }
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
                Debug.Log("Password reset email was canceled");
                textNotifyLoginEmailPassword.text = "Password reset email was canceled";
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
