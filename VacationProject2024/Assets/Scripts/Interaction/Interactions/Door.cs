using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractionAgent
{
    bool isOpened = false;
    Animation anim;
    
    protected void Start()
    {
        base.Start();
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnInteraction()
    {
        base.OnInteraction();

        if (isOpened)
        {
            anim.Play("Door2_Close");
            isOpened = false;
            feedbackText = "�� ����";
        }

        else
        {
            anim.Play("Door2_Open");
            isOpened = true;
            feedbackText = "�� �ݱ�";
        }
        Invoke("ReEnableInteraction", 0.2f);
    }

    void ReEnableInteraction()
    {
        AllowInteraction = true;
        Debug.Log("Enabled");
    }
}
