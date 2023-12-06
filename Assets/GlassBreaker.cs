using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBreaker : MonoBehaviour
{
    // Start is called before the first frame update
    public Material Glass1;
    public Material Glass2;
    public Material Glass3;
    public RockThrower Player;

    float health;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Player.health == 3)
        {
            GetComponent<Renderer>().material = Glass1;
        }else if(Player.health == 2)
        {
            GetComponent<Renderer>().material = Glass2;
        }
        else if(Player.health == 1)
        {
            GetComponent<Renderer>().material = Glass3;
        }
        
    }
}
