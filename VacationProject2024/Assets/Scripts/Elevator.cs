using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour, ISavable
{
    public Vector3 downPos, upPos;
    Animation anim;
    public bool isUsed = false;
    // Start is called before the first frame updat
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        GameManager.Instance.onGeneratorOn += OpenDoor;
        anim.Play("Elevator_Close");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        anim.Play("Elevator_Open");
    }

    public IEnumerator MoveUp()
    {
        anim.Play("Elevator_Close");
        SoundManager.Instance.PlaySound(gameObject, "Ev1", 1, 1);
        yield return new WaitForSeconds(2);
        isUsed = true;
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

    public void LoadData(Database data)
    {
        isUsed = data.elevatorMoved;
        if(isUsed)
        {
            transform.position = upPos;
            anim.Play("Elevator_Open");
        }
    }

    public void SaveData(ref Database data)
    {
        data.elevatorMoved  = isUsed;
    }
}
