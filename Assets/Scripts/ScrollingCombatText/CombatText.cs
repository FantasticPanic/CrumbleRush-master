using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*Put this script on a UI Prefab*/
public class CombatText : MonoBehaviour
{
    private float speed;
    private Vector3 direction;
    private float fadeTime;
    public AnimationClip critnAnim;
    private bool crit;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(2 * transform.position - CombatTextManager.Instance.camTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!crit)
        {
            // multiplying something by Time.deltaTime is how you get fluid movement it seems
            float translation = speed * Time.deltaTime;
            //move the text my the direction multipled by the translation
            transform.Translate(direction * translation);
        }
    }


    //according to the CombatTextManager
    public void Initialize(float speed, Vector3 direction, float fadeTime, bool crit)
    {

        // we say this. so that it accesses the fields up above
        this.speed = speed;
        this.direction = direction;
        this.fadeTime = fadeTime;
        this.crit = crit;
        if (crit)
        {
            GetComponent<Animator>().SetTrigger("Critical");
            StartCoroutine(Critical());
        }
        else
        {
            StartCoroutine(Fadeout());
        }
        //StartCoroutine(Fadeout());
    }


    private IEnumerator Critical()
    {

        //wait for the total duration of the animation
        crit = false;
        yield return new WaitForSeconds(critnAnim.length);
    }

    private IEnumerator Fadeout()
    {
        //get the alpha channel of our text mesh rpo
        float startAlpha = GetComponent<TextMeshProUGUI>().color.a;
        float rate = 1.0f / fadeTime;
        // 1.0 means it is full complete
        float progress = 0.0f;

        //while the progress is less than 1.0
        //the text should be fading
        while(progress < 1.0)
        {

            Color tmpColor = GetComponent<TextMeshProUGUI>().color;
            GetComponent<TextMeshProUGUI>().color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(startAlpha, 0, progress));

            progress += rate * Time.deltaTime;

            yield return null;
        }

        //Destroy the gameObject, AFTER it has faded away.
        Destroy(this.gameObject);

    }

   
}
