using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSize : MonoBehaviour
{
    public float max;
    public float min;
    float size;
    


    // Start is called before the first frame update
    void Start()
    {
        size = Random.Range(min, max);
        transform.localScale = new Vector3(transform.localScale.x * size, transform.localScale.y * size, transform.position.z * size);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
