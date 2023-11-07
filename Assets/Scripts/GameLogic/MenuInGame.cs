using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour
{
    [SerializeField]
    Button buttonMenu;
    [SerializeField]
    GameObject MenuManager;
    [SerializeField]
    Button buttonExit;

    private void Awake()
    {
        buttonMenu.onClick.AddListener(ShowMenuManager);
        buttonExit.onClick.AddListener(ShowMenuManager);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMenuManager()
    {
        MenuManager.SetActive(!MenuManager.activeSelf);
    }
}
