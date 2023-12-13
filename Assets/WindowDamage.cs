using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowDamage : MonoBehaviour
{
    public Material windowLightDamage;
    public Material windowMediumDamage;
    //public Texture windowCrackDamage;

    
    public void CrackWindow(float health)
    {
        Debug.Log("crack");
        if (health == 2)
        {
            GetComponent<Renderer>().material = windowLightDamage;
        }
        else if (health == 1)
        {
            GetComponent<Renderer>().material = windowMediumDamage;
        }
        else if (health == 0)
        {
            Debug.Log("dead 💀");
        }
    }
}
