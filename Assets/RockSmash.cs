using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RockSmash : MonoBehaviour
{
    ParticleSystem particle;
    Collider collider;
    MeshRenderer mr;
    WindowDamage windowDamage;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        collider = GetComponent<Collider>();
        mr = GetComponent<MeshRenderer>();
        windowDamage = gameObject.GetComponent<WindowDamage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
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
