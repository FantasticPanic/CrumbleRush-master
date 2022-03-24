using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameplayUIManager : MonoBehaviour
{
    private static GameplayUIManager instance;

    public static GameplayUIManager Instance
    {
        get
        {
            //if instance does not exist
            if (instance == null)
            {
                // find the GameplayUIManager and made a reference to it
                //we can access this in the PlayerDetection.cs script
                instance = FindObjectOfType<GameplayUIManager>();
            }
            return instance;
        }
    }
    [SerializeField]
    private GameObject warningIcon;
    [SerializeField]
    private GameObject warningText;
    public float verticalPunch;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        warningIcon.SetActive(false);
        warningText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //warningIcon.transform.DOPunchPosition(new Vector3(0, 5), 1f, 10, 1f, false);
    }

    public void SpeedPopup()
    {
        warningIcon.SetActive(true);
        warningText.SetActive(true);
        warningIcon.transform.GetComponent<Image>().DOFade(100f, .5f);
        warningText.transform.GetComponent<TextMeshProUGUI>().DOFade(100f, .5f);
        warningIcon.transform.DOPunchPosition(new Vector3(0, verticalPunch), 1f, 10, 1f, false);
        warningText.transform.DOPunchPosition(new Vector3(0, verticalPunch), 1f, 10, 1f, false);
        warningIcon.transform.GetComponent<Image>().DOFade(0f,3f);
        warningText.transform.GetComponent<TextMeshProUGUI>().DOFade(0f, 3f);
    }
}
