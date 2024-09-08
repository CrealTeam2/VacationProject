using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Vector3 downPos, upPos;
    Animation anim;
    // Start is called before the first frame updat
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        anim.Play("Elevator_Close");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator MoveUp()
    {
        anim.Play("Elevator_Close");
        SoundManager.Instance.PlaySound(gameObject, "Ev1", 1, 1);
        yield return new WaitForSeconds(2);
        float curTime = 0;
        while (true)
        {
            curTime += Time.deltaTime;
            transform.position = Vector3.Lerp(downPos, upPos, curTime / 10);
            if (curTime > 10) break;
            yield return null;
        }
        SoundManager.Instance.StopSound(gameObject, "Ev1");
        anim.Play("Elevator_Open");
    }
}
