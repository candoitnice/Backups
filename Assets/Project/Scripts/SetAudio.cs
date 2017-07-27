using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetAudio : MonoBehaviour {

    public AudioSource[] aud_BackG;
    public AudioSource[] aud_Source;

    public Slider backGSlider;
    public Toggle backToggle;

    public Slider sourceSlider;
    public Toggle sourceToggle;
    // Use this for initialization
    void Start ()
    { 	
	}

    public void SetBackAudioSource()
    {
        for (int i = 0; i < aud_BackG.Length; i++)
        {
            if(backToggle.isOn)
               aud_BackG[i].Play();
            else
                aud_BackG[i].Stop();
        }
    }
    public void SetBackVolume()
    {
        for (int i = 0; i < aud_BackG.Length; i++)
        {
            aud_BackG[i].volume=backGSlider.value;
        }
    }



    public void SetSourceAudioSource()
    {
        for (int i = 0; i < aud_Source.Length; i++)
        {
            if (sourceToggle.isOn)
                aud_Source[i].Play();
            else
                aud_Source[i].Stop();
        }
    }
    public void SetSourceVolume()
    {
        for (int i = 0; i < aud_Source.Length; i++)
        {
            aud_Source[i].volume = sourceSlider.value;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
