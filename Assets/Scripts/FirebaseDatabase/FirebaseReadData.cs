using Firebase;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;

public enum ReadDataType
{
    Map,
    User,
    Animal,
    AllUser,
    Chat
}

public class FirebaseReadData : MonoBehaviour
{
    DatabaseReference reference;

    public void ReadData(string path, ReadDataCallback callback, ReadDataType dataType)
    {
        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        //{
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
                                Debug.Log("Call date Maps");
                                callback.OnReadDataMapCompleted(json);
                                break;
                            case ReadDataType.User:
                                Debug.Log("Call data Users");
                                callback.OnReadDataUserCompleted(json);
                                break;
                            case ReadDataType.Animal:
                                Debug.Log("Call data Animals");
                                callback.OnReadDataAnimalCompleted(json);
                                break;
                            case ReadDataType.AllUser:
                                List<string> allUsers = new List<string>();
                                foreach (DataSnapshot ds in task.Result.Children)
                                {
                                    string user = ds.Value.ToString();
                                    allUsers.Add(user);
                                }
                                callback.OnReadDataAllUserCompleted(allUsers);
                                break;
                            case ReadDataType.Chat:
                                callback.OnReadDataChatCompleted(json);
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
        //});
    }
}
