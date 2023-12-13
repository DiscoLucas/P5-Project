using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowDamage : MonoBehaviour
{
    public Texture windowLightDamage;
    public Texture windowMediumDamage;
    public Texture windowCrackDamage;

    
    internal void CrackWindow(float health)
    {
        Material mat = GetComponent<Renderer>().material;
        if (health == 2)
        {
            mat.SetTexture("_MainTex", windowLightDamage);
        }
        else if (health == 1)
        {
            mat.SetTexture("_MainTex", windowMediumDamage);
        }
        else if (health == 0)
        {
            mat.SetTexture("_MainTex", windowCrackDamage);
        }
    }
}
