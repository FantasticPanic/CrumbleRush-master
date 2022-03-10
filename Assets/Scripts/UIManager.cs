using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            //if instance does not exist
            if (instance == null)
            {
                // find the UIManager and made a reference to it
                //we can access this in the PlayerDetection.cs script
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }


    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOn(GameObject uiElement)
    {
        uiElement.SetActive(true);
    }

    public void TurnOff(GameObject uiElement)
    {
        uiElement.SetActive(false);
    }
}
