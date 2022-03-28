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
    private GameObject player;


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
        player.transform.GetComponent<Customizable>().Customizations[0]._materialIndex = PlayerPrefs.GetInt("BodyVal");
        player.transform.GetComponent<Customizable>().Customizations[1]._subObjectIndex = PlayerPrefs.GetInt("HatVal");
        Debug.Log("Hat index is " +player.transform.GetComponent<Customizable>().Customizations[1]._subObjectIndex);
        Debug.Log("Body index is "+ player.transform.GetComponent<Customizable>().Customizations[0]._materialIndex);
        player.transform.GetComponent<Customizable>().Customizations[0].UpdateRenderers();
        player.transform.GetComponent<Customizable>().Customizations[1].UpdateSubObjects();
       // player.GetComponent<TrailRenderer>().material = player.transform.GetComponent<Customizable>().Customizations[0].Materials[PlayerPrefs.GetInt("BodyVal")];


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
        PlayerPrefs.SetInt("BodyVal", player.transform.GetComponent<Customizable>().Customizations[0]._materialIndex);
        PlayerPrefs.SetInt("HatVal", player.transform.GetComponent<Customizable>().Customizations[1]._subObjectIndex);
        PlayerPrefs.Save();
        Debug.Log("saving body index "+ player.transform.GetComponent<Customizable>().CurrentCustomization._materialIndex);
        Debug.Log("saving hat index" + player.transform.GetComponent<Customizable>().CurrentCustomization._subObjectIndex);
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
        player.transform.GetComponent<Customizable>()._currentCustomizationIndex = 1;
    }

    public void TransitionBody()
    {
        //customizeCamera.transform.position = bodyCamPosition.transform.position;
        customizeCamera.transform.DOMove(bodyCamPosition.transform.position, .5f, false);
        hatsBtn.SetActive(false);
        bodyBtn.SetActive(false);
        selectionArrows.SetActive(true);
        isCustomizing = true;
        player.transform.GetComponent<Customizable>()._currentCustomizationIndex = 0;
    }

    public void NextCosmetic()
    {
       player.transform.GetComponent<Customizable>().CurrentCustomization.NextSubObject();
       player.transform.GetComponent<Customizable>().CurrentCustomization.NextMaterial();
    }

    public void PreviousCosmetic()
    {
       player.transform.GetComponent<Customizable>().CurrentCustomization.PreviousSubObject();
       player.transform.GetComponent<Customizable>().CurrentCustomization.PreviousMaterial();
    }
}
