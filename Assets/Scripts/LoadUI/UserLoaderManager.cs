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
    [SerializeField]
    FirebaseWriteData firebaseWriteData;

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
        userInGame = JsonConvert.DeserializeObject<User>(data);
        textGold.text = userInGame.gold.ToString();
        textDiamond.text = userInGame.diamond.ToString();

        this.GetComponent<UnityMainThreadDispatcher>().Enqueue(() =>
        {
            textMeshProCharacterName.text = userInGame.characterName;
            if (userInGame.characterName == "")
            {
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
        firebaseWriteData.WriteData("Users/" + userInGame.id, userInGame.ToString());
    }
}
