using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : MonoBehaviour, ReadDataCallback
{
    [SerializeField]
    Text textGold;
    [SerializeField]
    Text textDiamond;

    FirebaseUser firebaseUser;
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
        firebaseReadData = FindObjectOfType<FirebaseReadData>();
        if (firebaseReadData == null)
        {
            Debug.LogError("FirebaseReadData component not found!");
        }
        else
        {
            if (firebaseUser != null)
            {
                firebaseReadData.ReadData("Users/" + firebaseUser.UserId, this);
                FirebaseDatabase.DefaultInstance.GetReference("Users/" + firebaseUser.UserId).ValueChanged += OnDataChanged;
            }
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
        firebaseReadData.ReadData("Users/" + firebaseUser.UserId, this);
    }

    public void OnReadDataCompleted(string data)
    {
        userInGame = JsonConvert.DeserializeObject<User>(data);
        textGold.text = userInGame.gold.ToString();
        textDiamond.text = userInGame.diamond.ToString();
    }
}
