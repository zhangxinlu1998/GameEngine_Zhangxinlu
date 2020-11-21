using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControler : MonoBehaviour
{
    public ParticleSystem particleLeft;
    public ParticleSystem particleRight;
    public ParticleSystem carAirTail;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    particleLeft.Play();
        //}

        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    particleRight.Play();
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    carAirTail.Play();
        //}
        //else if (Input.GetKeyUp(KeyCode.W))
        //{
        //    carAirTail.Stop();
        //}

        MoveCtrl(KeyCode.A, particleLeft);
        MoveCtrl(KeyCode.D, particleRight);
        MoveCtrl(KeyCode.W, carAirTail);

    }

    void MoveCtrl(KeyCode keyCode,ParticleSystem particle)
    {
        if (Input.GetKeyDown(keyCode))
        {
            particle.Play();
        }
        else if (Input.GetKeyUp(keyCode))
        {
            particle.Stop();
        }
    }
}