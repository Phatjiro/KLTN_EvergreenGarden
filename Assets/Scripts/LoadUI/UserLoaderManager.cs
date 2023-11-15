using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using PimDeWitte.UnityMainThreadDispatcher;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserLoaderManager : MonoBehaviour, ReadDataCallback
{
    public static bool isLoadedUser = false;

    [SerializeField]
    Text textGold;
    [SerializeField]
    Text textDiamond;
    [SerializeField]
    TextMeshPro textMeshProCharacterName;

    FirebaseUser firebaseUser;
    [SerializeField]
    FirebaseReadData firebaseReadData;

    public User userInGame;

    [SerializeField]
    GameObject wizardCharacterName;


    private void Awake()
    {
        // Get user already login
        firebaseUser = FirebaseAuth.DefaultInstance.CurrentUser;

        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (firebaseUser != null)
        {
            firebaseReadData.ReadData("Users/" + firebaseUser.UserId, this, ReadDataType.User);
            FirebaseDatabase.DefaultInstance.GetReference("Users/" + firebaseUser.UserId).ValueChanged += OnDataChanged;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDataChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        firebaseReadData.ReadData("Users/" + firebaseUser.UserId, this, ReadDataType.User);
    }

    public void OnReadDataUserCompleted(string data)
    {
        Debug.Log("Read User data");
        userInGame = JsonConvert.DeserializeObject<User>(data);
        Debug.Log("1");
        textGold.text = userInGame.gold.ToString();
        Debug.Log("2");
        textDiamond.text = userInGame.diamond.ToString();
        Debug.Log("3");

        this.GetComponent<UnityMainThreadDispatcher>().Enqueue(() =>
        {
            textMeshProCharacterName.text = userInGame.characterName;
            Debug.Log("char name: " + userInGame.characterName);
            if (userInGame.characterName == "")
            {
                Debug.Log("4");
                wizardCharacterName.SetActive(true);
            }
            isLoadedUser = true;
        });
    }

    public void OnReadDataMapCompleted(string data)
    {
        return;
    }

    public void SetDisplayName(string name)
    {
        this.userInGame.characterName = name;
        textMeshProCharacterName.text = name;
    }

   
}
