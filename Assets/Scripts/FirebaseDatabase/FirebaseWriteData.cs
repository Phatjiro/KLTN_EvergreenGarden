using Firebase;
using Firebase.Database;
using UnityEngine;

public class FirebaseWriteData : MonoBehaviour
{
    DatabaseReference reference;

    public void WriteData(string path, string dataToWrite)
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            Debug.Log("Write data to:" + path + "    " + dataToWrite);
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            reference.Child(path).SetValueAsync(dataToWrite);
        });
    }
}
