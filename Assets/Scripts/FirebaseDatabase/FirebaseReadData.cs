using Firebase;
using Firebase.Database;
using UnityEngine;

public enum ReadDataType
{
    Map,
    User,
    Animal
}

public class FirebaseReadData : MonoBehaviour
{
    DatabaseReference reference;

    public void ReadData(string path, ReadDataCallback callback, ReadDataType dataType)
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;

            reference.Child(path).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("Error getting data from Firebase: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        string json = snapshot.Value.ToString();
                        if (callback != null)
                        {
                            switch (dataType)
                            {
                                case ReadDataType.Map:
                                    callback.OnReadDataMapCompleted(json);
                                    break;
                                case ReadDataType.User:
                                    Debug.Log("Call data user");
                                    callback.OnReadDataUserCompleted(json);
                                    break;
                                case ReadDataType.Animal:
                                    Debug.Log("Call data Animal");
                                    callback.OnReadDataAnimalCompleted(json);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("No data available in Firebase");
                    }
                }
            });
        });
    }
}
