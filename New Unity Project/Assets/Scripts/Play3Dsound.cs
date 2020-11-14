using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play3Dsound : MonoBehaviour
{
    public Transform mainCamTrans;
    public float moveArea = 10;
    public float moveSpeed = 10;
    Transform thisTransform;
    private uint playingID = 0;
    Vector3 dirPos = Vector3.zero;
    bool dirBool = false;

    private void Start()
    {
        thisTransform = this.transform;
        thisTransform.position = mainCamTrans.position + new Vector3(0, -2, -3);
        dirPos = thisTransform.position;

    }
    

    // Update is called once per frame
    void Update()
    {
        if (dirBool)
        {
            if (Vector3.Distance(thisTransform.position, dirPos + new Vector3(moveArea, 0, 0)) < 0.1f)
                dirBool = false;
            thisTransform.position = Vector3.MoveTowards(thisTransform.position, dirPos + new Vector3(moveArea, 0, 0), moveSpeed * Time.deltaTime);
        }
        else
        {
            if (Vector3.Distance(thisTransform.position, dirPos + new Vector3(-moveArea, 0, 0)) < 0.1f)
                dirBool = true;
            thisTransform.position = Vector3.MoveTowards(thisTransform.position, dirPos + new Vector3(-moveArea, 0, 0), moveSpeed * Time.deltaTime);
        }

        if (playingID == 0)
        {
            playingID = AkSoundEngine.PostEvent("Play_ShortGun", this.gameObject);
        }
        
    }
}

