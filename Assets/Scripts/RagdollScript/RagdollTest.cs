using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollTest : MonoBehaviour
{
    public Rigidbody RIGID_BODY;
    public Animator anim;
    //a public area list of this gameobject's ragdoll colliders
    public List<Collider> RagdollParts = new List<Collider>();

    void Awake()
    {
        //SetRagdollParts();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Dies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetRagdollParts()
    {
        //set an array for all of the colliders that are children of this gameobject
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();
        //if a collider is not a collider of this gameobject in the hierarchy
        foreach (Collider c in colliders)
        {
            if (c.gameObject != this.gameObject)
            {
                //c.isTrigger = true;
                RagdollParts.Add(c);
            }

        }
    }

    private IEnumerator Dies()
    {
        yield return new WaitForSeconds(3f);
        TurnOnRagdoll();
    }

    public void TurnOnRagdoll()
    {
        RIGID_BODY.useGravity = false;
        RIGID_BODY.velocity = Vector3.zero;
        //this.gameObject.GetComponent<Collider>().enabled = false;
        anim.enabled = false;
        //anim.avatar = null;

        foreach (Collider c in RagdollParts)
        {
            c.attachedRigidbody.velocity = Vector3.zero;
           // c.isTrigger = false;
        }
    }
}
