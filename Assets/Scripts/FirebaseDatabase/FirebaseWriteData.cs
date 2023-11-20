using Firebase;
using Firebase.Database;
using UnityEngine;

public class FirebaseWriteData : MonoBehaviour
{
    DatabaseReference reference;

    public void WriteData(string path, string dataToWrite)
    {
#if UNITY_EDITOR
        return;
#endif
        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        //{
        FirebaseApp app = FirebaseApp.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child(path).SetValueAsync(dataToWrite);
        //});
    }
}
