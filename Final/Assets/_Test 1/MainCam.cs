using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    Transform thisTransform;

    public float speed = 3;
    public Vector3[] statePos;
    public Vector3[] stateRot;

    Vector3 defaultCamPos;
    Quaternion defaultCamRot;

    private void Start()
    {
        thisTransform = this.transform;
        defaultCamPos = this.transform.position;
        defaultCamRot = this.transform.rotation;
    }

    public float easeInOutQuart(float x) 
    {
        
        if (x < 0.5f)
            return 8 * x * x * x * x;
        else
            return 1 - Mathf.Pow(-2 * x + 2, 4) / 2;
    }

public IEnumerator Rot(int state)
{
    Vector3 tempPos = thisTransform.position;
    Quaternion tempRot = thisTransform.rotation;
    float timer = 0;
    switch (state)
    {
        case 0: //0为复位
            while (timer < 1)
            {
                timer += speed * Time.deltaTime * 2;
                thisTransform.position = Vector3.Lerp(tempPos, defaultCamPos, easeInOutQuart(timer));
                thisTransform.rotation = Quaternion.Slerp(tempRot, defaultCamRot, easeInOutQuart(timer));
                yield return null;
            }
            break;
        case 1:
            while (timer < 1)
            {
                timer += speed * Time.deltaTime;
                thisTransform.position = Vector3.Lerp(tempPos, statePos[0], easeInOutQuart(timer));
                thisTransform.rotation = Quaternion.Slerp(tempRot, Quaternion.Euler(stateRot[0]), easeInOutQuart(timer));
                yield return null;
            }
            break;
        case 2:
            while (timer < 1)
            {
                timer += speed * Time.deltaTime;
                thisTransform.position = Vector3.Lerp(tempPos, statePos[1], easeInOutQuart(timer));
                thisTransform.rotation = Quaternion.Slerp(tempRot, Quaternion.Euler(stateRot[1]), easeInOutQuart(timer));
                yield return null;
            }
            break;
        case 3:
            while (timer < 1)
            {
                timer += speed * Time.deltaTime;
                thisTransform.position = Vector3.Lerp(tempPos, statePos[2], easeInOutQuart(timer));
                thisTransform.rotation = Quaternion.Slerp(tempRot, Quaternion.Euler(stateRot[2]), easeInOutQuart(timer));
                yield return null;
            }
            break;
        case 4:
            while (timer < 1)
            {
                timer += speed * Time.deltaTime;
                thisTransform.position = Vector3.Lerp(tempPos, statePos[3], easeInOutQuart(timer));
                thisTransform.rotation = Quaternion.Slerp(tempRot, Quaternion.Euler(stateRot[3]), easeInOutQuart(timer));
                yield return null;
            }
            break;
    }
    yield return null;
}
}