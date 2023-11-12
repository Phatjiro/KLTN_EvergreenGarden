using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class UserLoaderManager : MonoBehaviour, ReadDataCallback
{
    [SerializeField]
    Text textGold;
    [SerializeField]
    Text textDiamond;

    FirebaseUser firebaseUser;
    [SerializeField]
    FirebaseReadData firebaseReadData;

    User userInGame;

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
    }

    public void OnReadDataMapCompleted(string data)
    {
        return;
    }
}
