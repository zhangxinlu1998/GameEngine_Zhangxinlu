using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMusic : MonoBehaviour
{

    bool trigger = true;

    void Start()
    {
        
    }

    
    public void Play()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");

        if (trigger)
        {
            AkSoundEngine.PostEvent("Play_Music", mainCamera);
            trigger = false;
        }
        else
        {
            AkSoundEngine.PostEvent("Pause_Music", mainCamera);
            trigger = true;
        }




    }
}
