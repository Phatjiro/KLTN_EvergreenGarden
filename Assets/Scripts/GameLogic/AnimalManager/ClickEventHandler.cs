using UnityEngine;
using UnityEngine.UI;

public class ClickEventHandler : MonoBehaviour
{
    private GameObject objectCanvas;
    private AnimalInforManager  animalInfor;
 
    private void Start()
    {
        objectCanvas = GameObject.FindGameObjectWithTag("Canvas");
        
        if (objectCanvas != null ) 
        {
            animalInfor = objectCanvas.transform.Find("AnimalInfor").gameObject.GetComponent<AnimalInforManager>();
        }
        
    }

    public void ActiveCanvas()
    {
        Debug.Log("Show animal infor");
        Animal infor = this.GetComponent<AnimalLivingInformation>().information;

        animalInfor.gameObject.SetActive(true);
        animalInfor.loadAnimalInfor(infor, this.gameObject);
    }


}
