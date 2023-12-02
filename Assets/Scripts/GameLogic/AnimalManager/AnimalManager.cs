using Firebase.Auth;
using HappyHarvest;
using PimDeWitte.UnityMainThreadDispatcher;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnimalManager : MonoBehaviour
{
    [SerializeField]
    private GameObject chickenPrefab;

    [SerializeField]
    private GameObject positionChicken;

    [SerializeField]
    private BoxCollider2D boxColliderChicken;

    [SerializeField]
    private BredAnimalDBManager bredAnimalDBManager;

    private FirebaseUser firebaseUser;
    [SerializeField]
    private GameObject piggyPrefab;
    [SerializeField]
    private GameObject positionPiggy;
    [SerializeField]
    private BoxCollider2D boxColliderPiggy;  
    
    [SerializeField]
    private GameObject cowPrefab;
    [SerializeField]
    private GameObject positionCow;
    [SerializeField]
    private BoxCollider2D boxColliderCow;

  
    private void Awake()
    {

    }

    public void InstanceNewCow()
    {
        GameObject go = Instantiate(cowPrefab);
        float randomX = UnityEngine.Random.Range(-4, 4);
        float randomY = UnityEngine.Random.Range(-3, 3);
        go.transform.position = positionCow.transform.position + new Vector3(randomX, randomY, 0);
        go.GetComponent<BasicAnimalMovement>().Area = boxColliderCow;

        var newCow = new Animal("Cow", DateTime.Now, 100, 150, 600);
        SetInforAnimal(go, newCow);
        Debug.Log("tao con bo");
        bredAnimalDBManager.addAnimal(newCow);
        bredAnimalDBManager.addToDB();
    }

    public void InstanceNewChicken()
    {
        GameObject go = Instantiate(chickenPrefab);
        float randomX = UnityEngine.Random.Range(-4, 4);
        float randomY = UnityEngine.Random.Range(-1, 1);
        go.transform.position = positionChicken.transform.position + new Vector3(randomX, randomY, 0);
        go.GetComponent<BasicAnimalMovement>().Area = boxColliderChicken;

        var newchicken = new Animal("Chicken", DateTime.Now, 30, 50, 200);
        SetInforAnimal(go, newchicken);
        Debug.Log("tao con ga");
        bredAnimalDBManager.addAnimal(newchicken);
        bredAnimalDBManager.addToDB();
       
    }


    public void InstanceNewPiggy()
    {
        GameObject go = Instantiate(piggyPrefab);
        float randomX = UnityEngine.Random.Range(-4, 4);
        float randomY = UnityEngine.Random.Range(-1, 1);
        go.transform.position = positionPiggy.transform.position + new Vector3(randomX, randomY, 0);
        go.GetComponent<BasicAnimalMovement>().Area = boxColliderPiggy;

        var newPiggy = new Animal("Piggy", DateTime.Now, 50, 100, 400);
        SetInforAnimal(go,newPiggy);
        Debug.Log("tao con heo");
        bredAnimalDBManager.addAnimal(newPiggy);
        bredAnimalDBManager.addToDB();
    }
    public void SetInforAnimal(GameObject go, Animal infor)
    {
        if (go.GetComponent<AnimalLivingInformation>() != null)
        {
            go.GetComponent<AnimalLivingInformation>().information = infor;
        }
    }

    public void InstanceCowFromDB(Animal nCow)
    {
        this.GetComponent<UnityMainThreadDispatcher>().Enqueue(() =>
        {
            GameObject go = Instantiate(cowPrefab);
            float randomX = UnityEngine.Random.Range(-4, 4);
            float randomY = UnityEngine.Random.Range(-3, 3);
            go.transform.position = positionCow.transform.position + new Vector3(randomX, randomY, 0);
            go.GetComponent<BasicAnimalMovement>().Area = boxColliderCow;
            SetInforAnimal(go, nCow);
        });

    }

    public void InstanceChickenFromDB(Animal chick)
    {
        this.GetComponent<UnityMainThreadDispatcher>().Enqueue(() =>
        {
            GameObject go = Instantiate(chickenPrefab);
            float randomX = UnityEngine.Random.Range(-4, 4);
            float randomY = UnityEngine.Random.Range(-1, 1);
            go.transform.position = positionChicken.transform.position + new Vector3(randomX, randomY, 0);
            go.GetComponent<BasicAnimalMovement>().Area = boxColliderChicken;
            SetInforAnimal(go, chick);
        });

    }
    public void InstancePiggyFromDB(Animal pig)
    {
        this.GetComponent<UnityMainThreadDispatcher>().Enqueue(() =>
        {
            GameObject go = Instantiate(piggyPrefab);
            float randomX = UnityEngine.Random.Range(-4, 4);
            float randomY = UnityEngine.Random.Range(-1, 1);
            go.transform.position = positionPiggy.transform.position + new Vector3(randomX, randomY, 0);
            go.GetComponent<BasicAnimalMovement>().Area = boxColliderPiggy;
            SetInforAnimal(go, pig);
        });

    }

    public void LoadAnimal(List<Animal> lstAnimal)
    {
        foreach (Animal animal in lstAnimal)
        {
            if (animal.name.Equals("Chicken"))
            {
                InstanceChickenFromDB(animal);
            } else if (animal.name.Equals("Piggy"))
            {
                InstancePiggyFromDB(animal);
            }else if (animal.name.Equals("Cow"))
            {
                InstanceCowFromDB(animal);
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);
            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != null)
                {
                    if (hit.collider.gameObject.tag == "Animal")
                    {
                        hit.collider.gameObject.GetComponent<ClickEventHandler>().ActiveCanvas();
                        break;
                    }
                }
            }
        }
    }
}
