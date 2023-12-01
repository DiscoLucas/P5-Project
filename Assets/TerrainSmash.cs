using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainSmash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ship VR Variant") //KÆMPE FEJLKILDE PAS PÅ MED AT RENAME SKIBET
        {
            collision.gameObject.GetComponent<RockThrower>().health-=3;
        }
    }
}
