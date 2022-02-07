using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Put this script on the player*/
public class CombatPlayer : MonoBehaviour
{
    public float speed;

    private bool onCooldown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        transform.Translate(new Vector3(horizontal, vertical) * (speed * Time.deltaTime));


        if (Input.GetKeyDown(KeyCode.Space))
        {
            CombatTextManager.Instance.CreateText(transform.position, "Regular", Color.white, false);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            CombatTextManager.Instance.CreateText(transform.position, "HOT POCKET", Color.red, true);
        }
    }


    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Hazard")
        {
            if (!onCooldown)
            {
                StartCoroutine(Cooldown());
                int random = Random.Range(0, 2);

                if (random == 0)
                {
                    int randomDamage = Random.Range(3, 10);
                    CombatTextManager.Instance.CreateText(transform.position, "-" + randomDamage.ToString(), Color.red, false);
                }
                else
                {
                    int randomDamage = Random.Range(11, 20);
                    CombatTextManager.Instance.CreateText(transform.position, "-" + randomDamage.ToString(), Color.red, true);

                }
            }
        }
        
        else if (col.tag == "Heal")
        { 
            if (!onCooldown)
            {

            }
       }
    }

    private IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(2);
        onCooldown = false;
    }
}
