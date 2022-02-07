using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{

    private ParticleSystem ps;

   
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        //at some point the partcile stops playing
        //when that happens, destroy it
        StartCoroutine(DestroyParticles(5.0f));
       
        
    }

    private IEnumerator DestroyParticles(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);


    }
}
