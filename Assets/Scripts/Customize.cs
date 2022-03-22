using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Customize : MonoBehaviour
{
    [SerializeField]
    private GameObject customizeCamera;
    [SerializeField]
    private GameObject startMenu;
    [SerializeField]
    private GameObject score;

    [SerializeField]
    private GameObject hatsCamPosition;
    [SerializeField]
    private GameObject bodyCamPosition;
    [SerializeField]
    private GameObject defaultCamPosition;

    [SerializeField]
    private GameObject selectionArrows;
    [SerializeField]
    private GameObject hatsBtn;
    [SerializeField]
    private GameObject bodyBtn;
    private bool isCustomizing;

    [SerializeField]
    private GameObject Player;


    private static Customize instance;

    public static Customize Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Customize>();
            }
            return instance;
        }
    }
    

    private void Awake()
    {
        //startMenu = GameObject.Find("StartMenu").gameObject;
        //score = GameObject.Find("Score").gameObject;
        //hatsCamPosition = GameObject.Find("HatsCamPosition").gameObject;
       // bodyCamPosition = GameObject.Find("BodyCamPosition").gameObject;
    }

    void Start()
    {
        selectionArrows.SetActive(false);
        isCustomizing = false;
     

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

    public void GoBack()
    {
        customizeCamera.transform.DOMove(defaultCamPosition.transform.position, .5f, false);
        hatsBtn.SetActive(true);
        bodyBtn.SetActive(true);
        selectionArrows.SetActive(false);
        isCustomizing = false;
    }

    public void CustomizeBackButton()
    {
        if (isCustomizing)
        {
            GoBack();
        }
        else
        {
            TurnOff();
        }
    }

    public void TransitionHats()
    {
        //customizeCamera.transform.position = hatsCamPosition.transform.position;
        customizeCamera.transform.DOMove(hatsCamPosition.transform.position, .5f, false);
        hatsBtn.SetActive(false);
        bodyBtn.SetActive(false);
        selectionArrows.SetActive(true);
        isCustomizing = true;
        Player.transform.GetComponent<Customizable>()._currentCustomizationIndex = 1;
    }

    public void TransitionBody()
    {
        //customizeCamera.transform.position = bodyCamPosition.transform.position;
        customizeCamera.transform.DOMove(bodyCamPosition.transform.position, .5f, false);
        hatsBtn.SetActive(false);
        bodyBtn.SetActive(false);
        selectionArrows.SetActive(true);
        isCustomizing = true;
        Player.transform.GetComponent<Customizable>()._currentCustomizationIndex = 0;
    }

    public void NextCosmetic()
    {
       Player.transform.GetComponent<Customizable>().CurrentCustomization.NextSubObject();
       Player.transform.GetComponent<Customizable>().CurrentCustomization.NextMaterial();
    }

    public void PreviousCosmetic()
    {
       Player.transform.GetComponent<Customizable>().CurrentCustomization.PreviousSubObject();
       Player.transform.GetComponent<Customizable>().CurrentCustomization.PreviousMaterial();
    }
}
