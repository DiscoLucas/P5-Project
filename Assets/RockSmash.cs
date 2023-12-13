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
        windowDamage = FindObjectOfType<WindowDamage>();
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
            windowDamage.CrackWindow(collision.gameObject.GetComponent<RockThrower>().health);
            Debug.Log((collision.gameObject.GetComponent<RockThrower>()!=null)+ " " + (collision.gameObject.GetComponent<RockThrower>().health));
            Debug.Log((windowDamage != null));
            AudioManager.instance.Play("RockBreak");
            AudioManager.instance.Play("Rock crumble");
        }
    }
}
