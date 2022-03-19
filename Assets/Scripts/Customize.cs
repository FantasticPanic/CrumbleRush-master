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

    private void Awake()
    {
        startMenu = GameObject.Find("StartMenu").gameObject;
        score = GameObject.Find("Score").gameObject;
        hatsCamPosition = GameObject.Find("HatsCamPosition").gameObject;
        bodyCamPosition = GameObject.Find("BodyCamPosition").gameObject;
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

    public void TransitionHats()
    {
        //customizeCamera.transform.position = hatsCamPosition.transform.position;
        customizeCamera.transform.DOMove(hatsCamPosition.transform.position, .5f, false);
    }

    public void TransitionBody()
    {
        //customizeCamera.transform.position = bodyCamPosition.transform.position;
        customizeCamera.transform.DOMove(bodyCamPosition.transform.position, .5f, false);
    }
}
