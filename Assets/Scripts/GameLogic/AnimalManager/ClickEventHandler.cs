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

    private void OnMouseDown()
    {
        if (animalInfor.gameObject.activeSelf)
        {
            // Nếu đã hiển thị, ẩn UI Canvas
            animalInfor.gameObject.SetActive(false);
        }
        else
        {
            // Nếu chưa hiển thị, hiển thị UI Canvas
            Animal infor = this.GetComponent<AnimalLivingInformation>().information;

            animalInfor.gameObject.SetActive(true);
            animalInfor.loadAnimalInfor(infor, this.gameObject);
        }

    }


}
