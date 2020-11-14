using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUI : MonoBehaviour
{
    void Start()
    {
        
    }

    
    public void Play()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");

        AkSoundEngine.PostEvent("Play_UI_Click", mainCamera);

    }
}
