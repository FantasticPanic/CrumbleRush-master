using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customize : MonoBehaviour
{
    [SerializeField]
    private GameObject customizeCamera;
    [SerializeField]
    private GameObject startMenu;
    private GameObject score;

    private void Awake()
    {
        startMenu = GameObject.Find("StartMenu").gameObject;
        score = GameObject.Find("Score").gameObject;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOn()
    {
        // UIManager.Instance.TurnOn(customizeCamera);
        transform.GetChild(0).gameObject.SetActive(true);
        startMenu.SetActive(false);
        score.SetActive(false);

    }

    public void TurnOff()
    {
        //UIManager.Instance.TurnOff(customizeCamera);
        transform.GetChild(0).gameObject.SetActive(false);
        startMenu.SetActive(true);
        score.SetActive(true);
    }
}
