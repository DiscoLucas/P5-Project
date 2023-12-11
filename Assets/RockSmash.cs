using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RockSmash : MonoBehaviour
{
    ParticleSystem particle;
    CapsuleCollider collider;
    MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        collider = GetComponent<CapsuleCollider>();
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Ship VR Variant") //KÆMPE FEJLKILDE PAS PÅ MED AT RENAME SKIBET
        {
            mr.enabled = false;
            collider.enabled = false;
            particle.Play();
            collision.gameObject.GetComponent<RockThrower>().health--;
            FindObjectOfType<AudioManager>().Play("RockBreak");
            FindObjectOfType<AudioManager>().Play("Rock crumble");
        }
    }
}
