using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCollider : MonoBehaviour
{
    bool isOnElv;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isOnElv = true;
            StartCoroutine(GenerateElv());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isOnElv = false;
        }
    }

    IEnumerator GenerateElv()
    {
        int i = 0;
        while(i < 100)
        {
            if(!isOnElv) StopAllCoroutines();
            yield return null;
            i++;
        }
        if(isOnElv)
        StartCoroutine(transform.parent.parent.GetComponent<Elevator>().MoveUp());
    }
}
