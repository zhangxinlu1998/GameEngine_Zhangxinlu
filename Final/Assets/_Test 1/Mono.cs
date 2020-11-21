using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mono : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            if (collision.gameObject.GetComponent<Player>().playerState == 1)
                StartCoroutine(Destory());
    }

    IEnumerator Destory()
    {
        yield return new WaitForSeconds(0.05f);
        this.gameObject.SetActive(false);
    }
}