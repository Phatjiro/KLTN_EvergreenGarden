using Firebase;
using Firebase.Database;
using UnityEngine;

public class FirebaseWriteData : MonoBehaviour
{
    DatabaseReference reference;

    public void WriteData(string table, string dataToWrite)
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            reference.Child(table).SetValueAsync(dataToWrite);
        });
    }
}
