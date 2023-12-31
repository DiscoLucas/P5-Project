using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Voice
{

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [HideInInspector]
    public AudioSource source;
}
