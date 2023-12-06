using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	//Creats the sound array
	public Sound[] soundsArray;
	public Voice[] voiceArray;


    //Singleton pattern
    public static AudioManager instance;

    void Awake ()
	{
		//Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
			return;
        }

        foreach (Sound s in soundsArray)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;

			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
            s.source.loop = s.loop;
			s.source.spatialBlend = s.spatialBlend;
        }

		foreach (Voice v in voiceArray)
		{
            v.source = gameObject.AddComponent<AudioSource>();
            v.source.clip = v.clip;

            v.source.volume = v.volume;
        }
	}


	void start()
	{
		//Play("Wind");
       // Play("Propella");
    }

	//To call:
	//FindObjectOfType<AudioManager>().Play("clip_name");
	//
	public void Play (string name)
	{
		Sound s = Array.Find(soundsArray, soundsArray => soundsArray.name == name);

		if (s == null)
		{
			Debug.LogWarning(name + " kinda cringe, check spelling you dylexic ass");
			return;
		}
		s.source.Play();
	}
}
