using Firebase;
using Firebase.Database;
using UnityEngine;

public class FirebaseReadData : MonoBehaviour
{
    DatabaseReference reference;

    public void ReadData(string path, ReadDataCallback callback)
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
                            callback.OnReadDataCompleted(json);
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
