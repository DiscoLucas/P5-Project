using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchAddSpeed : MonoBehaviour
{
    public Collider c;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision == c)
        {
            collision.gameObject.GetComponent<RockThrower>().speed += collision.gameObject.GetComponent<RockThrower>().VelocAdd;
            collision.gameObject.GetComponent<RockThrower>().an.SetTrigger("trPlay");
            collision.gameObject.GetComponent<RockThrower>().deflateTime = 0.6f;
            Destroy(this);
        }
    }
}
