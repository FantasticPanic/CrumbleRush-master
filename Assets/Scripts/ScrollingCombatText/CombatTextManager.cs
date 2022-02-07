using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//put this script on a text manager

public class CombatTextManager : MonoBehaviour
{

    public static float health;

    /// <summary>
    /// The camera's transform
    /// </summary>
    public Transform camTransform;

    public GameObject textPrefab;

    public RectTransform canvasTransform;

    public float speed;
    public float fadeTime;
    public bool crit;
    public Vector3 direction;

    

    //singleton
    //we are able to access the CombatTextManager class anywhere through "instance"
    private static CombatTextManager instance;

    public static CombatTextManager Instance
    {
        get
        {
            if (instance == null)
            {
                //set instance to CombatTextManager if it is null
                instance = GameObject.FindObjectOfType<CombatTextManager>();
            }
            return instance;
        }
    }

    /*As you can see, createText method will have arguments for components of the INSTANTIATED version of the 
     prefab that we had in Unity.
         */

    // the argument is player's position
    //attach to the prefab that will be your UI text
    public void CreateText(Vector3 position, string text, Color color, bool crit)
    {
        //we cast the popUpText so that we can call on the instantiated version of the prefab
        //like it seems redundent, but actually, we are doing God's work by only manipulating
        //the version of the prefab that is instantiated, and not the prefab itself
       GameObject popUpText = (GameObject) Instantiate(textPrefab, position, Quaternion.identity);

        popUpText.transform.SetParent(canvasTransform);
       
        popUpText.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        //access the CombatText script component of the gameobject
        popUpText.GetComponent<CombatText>().Initialize(speed, direction, fadeTime, crit);
        popUpText.GetComponent<TextMeshProUGUI>().text = text;
        popUpText.GetComponent<TextMeshProUGUI>().color = color;
        
    }
    
}
